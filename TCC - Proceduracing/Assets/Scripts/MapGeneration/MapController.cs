using UnityEngine;

enum DisplayMode
{
    NoiseMap,
    VoronoiMap
}

public class MapController : MonoBehaviour
{
    [Header("General Settings")]
    [SerializeField] private GameObject terrain;
    [SerializeField] private Vector2Int mapSize;
    [SerializeField] private int seed;
    [SerializeField] private bool randomSeed;
    [SerializeField] private Vector2 offset;
    [SerializeField] private DisplayMode displayMode;
    [SerializeField] bool flatShading;

    private Vertex[,] vertexMap;

    [Header("Perlin Settings")]
    [SerializeField] private float noiseScale;
    [SerializeField] private float heightMultiplier;
    [SerializeField] private int octaves;
    [Range(0,1)]
    [SerializeField] private float persistence;
    [SerializeField] private float lacunarity;

    private Vertex[,] noiseMap;

    [Header("Voronoi Settings")]
    [SerializeField] private int regionAmount;
    [Range(0f, 1f)]
    [SerializeField] private float regionMinimumInfluence;

    private Vertex[,] voronoiMap;

    [Header("Internal")]

    private MeshFilter meshFilter;
    private MeshCollider meshCollider;
    private Renderer meshRenderer;

    private void Start()
    {
        GetAllComponents();
        GenerateMap();
    }

    public void GetAllComponents() {
        meshFilter = terrain.GetComponent<MeshFilter>();
        meshCollider = terrain.GetComponent<MeshCollider>();
        meshRenderer = terrain.GetComponent<Renderer>();
    }

    public void GenerateMap()
    {
        if (randomSeed)
            seed = Random.Range(0, 100000);
        vertexMap = new Vertex[mapSize.x, mapSize.y];
        noiseMap = PerlinNoise.GenerateNoiseMap(mapSize, seed, noiseScale, octaves, persistence, lacunarity, offset);
        voronoiMap = VoronoiNoise.GenerateNoiseMap(mapSize, seed, regionAmount, regionMinimumInfluence);

        AssignValuesToVertex();

        GenerateMesh();
        DisplayMap();

        PassMapToWheels();
    }

    private void PassMapToWheels()
    {
        FindObjectOfType<WheelController>().GetComponent<WheelController>().MapFrictionInfo = vertexMap;
    }

    private void AssignValuesToVertex()
    {
        for (int y = 0; y < mapSize.y; y++)
        {
            for (int x = 0; x < mapSize.x; x++)
            {
                vertexMap[x, y] = new Vertex()
                {
                    height = noiseMap[x, y].height,
                    biome = voronoiMap[x, y].biome,
                };
                //Debug.Log(vertexMap[x, y].ToString());
            }
        }
    }

    public void GenerateMesh()
    {
        Mesh mesh = MeshGenerator.GenerateTerrainMesh(vertexMap, heightMultiplier, flatShading);
        meshFilter.sharedMesh = mesh;
        meshCollider.sharedMesh = mesh;
    }

    public void DisplayMap() 
    {
        Texture2D texture = new Texture2D(mapSize.x, mapSize.y);

        switch (displayMode)
        {
            case DisplayMode.NoiseMap:
                texture.SetPixels32(MapDisplay.DrawNoiseMap(vertexMap));
                break;

            case DisplayMode.VoronoiMap:
                texture.SetPixels32(MapDisplay.DrawVoronoiMap(vertexMap));
                break;
        }

        texture.filterMode = FilterMode.Point;
        texture.Apply();

        meshRenderer.sharedMaterial.mainTexture = texture;
    }

    private void OnDrawGizmos()
    {
       /* for (int y = 0; y < vertexMap.GetLength(0); y++)
        {
            for (int x = 0; x < vertexMap.GetLength(1); x++)
            {
                //if (vertexMap[x, y].biome.friction == 0.5f) {
                    Gizmos.color = vertexMap[x, y].biome.colors[0].color;
                    Gizmos.DrawSphere(new Vector3(x, 0, y), 0.2f);
                //}
            }
        }
       */
    }
}
