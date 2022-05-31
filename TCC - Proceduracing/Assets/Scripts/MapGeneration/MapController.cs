using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;
using UnityEngine.SceneManagement;

enum DisplayMode
{
    NoiseMap,
    VoronoiMap
}

public class MapController : MonoBehaviour
{
    [Header("General Settings")]
    [SerializeField] private Transform car;
    [SerializeField] private GameObject terrain;
    [SerializeField] private Vector2Int mapSize;
    [SerializeField] public static int seed;
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
    [Range(1, 2)]
    [SerializeField] private int regionAmount;
    [SerializeField] private Vector2Int regionMinMaxRadius;
    [Range(0,1f)]
    [SerializeField] private float regionTransition;

    private Vertex[,] voronoiMap;

    [Header("Poisson Disk Settings")]

    [SerializeField] private float radius = 1;
    [SerializeField] private Vector2 regionSize = Vector2.one;
    [SerializeField] private int rejectionSamples = 30;
    [SerializeField] private float displayRadius = 1;
    [SerializeField] private Transform structureParent;

    private List<Vector2> poissonDiskPoints;

    [Header("Race Settings")]
    [SerializeField] private GameObject checkPointGameObject;
    [SerializeField] private int checkPointsAmount;
    [SerializeField] private float minDistanceBetweenPoints;
    [Range(0f, 1f)]
    [SerializeField] private float border;
    [SerializeField] private int checkPointsCollected;

    [Header("Internal")]

    private MeshFilter meshFilter;
    private MeshCollider meshCollider;
    private Renderer meshRenderer;

    [SerializeField] OpponentsController opponentsController;
    [SerializeField] TextMeshProUGUI checkPointsUI;

    private Vector2Int startPoint;

    private void Start()
    {
        randomSeed = QuickRace.randomSeed;
        seed = QuickRace.seed;

        GetAllComponents();
        GenerateMap();
        CheckPoint.CheckPointCaught += CheckPointCollected;
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
        var perlin = PerlinNoise.GenerateNoiseMap(mapSize, seed, noiseScale, octaves, persistence, lacunarity, offset);
        noiseMap = perlin.Item1;
        voronoiMap = VoronoiNoise.GenerateNoiseMap(mapSize, noiseMap, perlin.Item2, seed, regionAmount, regionMinMaxRadius, regionTransition);
        var points = RaceController.GenerateRace(vertexMap, seed, checkPointGameObject, checkPointsAmount, minDistanceBetweenPoints, border);
        startPoint = points[0];
        car.position = new Vector3(startPoint.x, car.position.y, startPoint.y);

        var averages = AssignValuesToVertex();
        var dist = CalculateDistanceBetweenCheckPoints(points);
        var time = CalculateRaceTime(dist, averages.Item1, averages.Item2);
        opponentsController.PassTime(seed, time);

        GenerateMesh();
        DisplayMap();
        StructuresPlacement();

        PassMapToWheels();

        UpdateCheckPointUI();
    }

    private void UpdateCheckPointUI()
    {
        checkPointsUI.text = checkPointsCollected + " / " + checkPointsAmount;
        // puxar pra prox tela
    }

    private float CalculateDistanceBetweenCheckPoints(Vector2Int[] checkPoints)
    {
        List<Vector2Int> possibleCheckPoints = checkPoints.ToList();
        List<Vector2Int> calculatedCheckPoints = new List<Vector2Int>
        {
            possibleCheckPoints[0]
        };
        possibleCheckPoints.RemoveAt(0);

        float finalDistance = 0;


        for (int i = 0; i < calculatedCheckPoints.Count; i++)
        {
            var distance = Mathf.Infinity;
            var index = 0;

            if(possibleCheckPoints.Count != 0) { 
                for (int j = 0; j < possibleCheckPoints.Count; j++)
                {
                    var checkPointsDistance = Vector2Int.Distance(calculatedCheckPoints[i], possibleCheckPoints[j]);

                    if (checkPointsDistance < distance)
                    {
                        distance = checkPointsDistance;
                        index = j;
                    }
                }
                calculatedCheckPoints.Add(possibleCheckPoints[index]);
                possibleCheckPoints.RemoveAt(index);
                finalDistance += distance;
            }
        }
        return finalDistance;
    }

    private float CalculateRaceTime(float distance, float averageHeight, float averageFriction)
    {
        float frictionMultiplier = averageFriction >= 1 ? 1f : 2f - averageFriction;
        Debug.Log(frictionMultiplier);
        float heightMultiplier = Mathf.Abs(averageHeight - 0.5f);
        Debug.Log(averageHeight);
        Debug.Log(heightMultiplier);
        float time = distance * (frictionMultiplier + heightMultiplier) / 10;
        return time;
    }

    private void StructuresPlacement()
    {
        poissonDiskPoints = PoissonDiscSampling.GeneratePoints(radius, regionSize, rejectionSamples);
        System.Random prgn = new System.Random(seed);

        if (poissonDiskPoints != null)
        {
            foreach (var point in poissonDiskPoints)
            {
                var vertex = vertexMap[Mathf.FloorToInt(point.x), Mathf.FloorToInt(point.y)];
                var biome = vertex.biomeList[prgn.Next(0, vertex.biomeList.Count)];
                var structure = biome.structures[prgn.Next(0, biome.structures.Length)];

                Instantiate(structure.structure, new Vector3(point.x, vertex.height * heightMultiplier, point.y), Quaternion.identity, structureParent);
            }
        }
    }

    private void PassMapToWheels()
    {
        FindObjectOfType<WheelController>().GetComponent<WheelController>().MapFrictionInfo = vertexMap;
    }

    private System.Tuple<float, float> AssignValuesToVertex()
    {
        float averageHeight = 0;
        float averageFriction = 0;
        var count = 0;

        for (int y = 0; y < mapSize.y; y++)
        {
            for (int x = 0; x < mapSize.x; x++)
            {
                vertexMap[x, y] = new Vertex()
                {
                    height = noiseMap[x, y].height,
                    friction = voronoiMap[x, y].friction,
                    color = voronoiMap[x, y].color,
                    biomeList = voronoiMap[x, y].biomeList,
                };

                count++;
                averageHeight = averageHeight * (count - 1) / count + noiseMap[x, y].height / count;
                averageFriction = averageFriction * (count - 1) / count + voronoiMap[x, y].friction / count;
            }
        }

        return new System.Tuple<float, float>(averageHeight, averageFriction);
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

    private void CheckPointCollected() {
        checkPointsCollected += 1;
        UpdateCheckPointUI();

        if (checkPointsCollected == checkPointsAmount)
        {
            SceneManager.LoadScene(2);
        }

        //Debug.Log(checkPointsCollected);
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawSphere(new Vector3(startPoint.x, 5, startPoint.y), 10f);

        if (poissonDiskPoints != null)
        {
            foreach (Vector2 point in poissonDiskPoints)
            {
                Gizmos.DrawSphere(new Vector3(point.x, 5, point.y), displayRadius);
            }
        }
    }
}