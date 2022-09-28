using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;
using UnityEngine.SceneManagement;

public class MapController : MonoBehaviour
{
    [SerializeField] private MapData mapData;
    [SerializeField] private PerlinData perlinData;
    [SerializeField] private VoronoiData voronoiData;
    [SerializeField] private PoissonDiskData poissonDiskData;
    [SerializeField] private RaceSettingsData raceSettingsData;

    [SerializeField] private WheelController wheelController;

    private Vertex[,] vertexMapData;

    [SerializeField] TextMeshProUGUI checkPointsUI;

    public static bool jaAtribuiu = false;

    private int Seed => GlobalSeed.Seed;

    private float _averageMapHeight;
    private float _averageMapFriction;

    private void Start()
    {
        GenerateMap();
    }

    public void GenerateMap()
    {
        vertexMapData = mapData.GetVertexMap();
        perlinData.Init(mapData.MapSize, Seed, mapData.Offset);

        voronoiData.Init(mapData.MapSize, perlinData.NoiseMap, perlinData.MinMax, Seed);
        raceSettingsData.Init(vertexMapData, Seed);
        AssignValuesToVertex();

        raceSettingsData.PassOpponentsTime(Seed, _averageMapHeight, _averageMapFriction);

        GenerateMesh();
        DisplayMap();

        StructuresPlacement();

        wheelController.Init(vertexMapData, raceSettingsData.CheckPointPosition[0]);

        UpdateCheckPointUI();
    }

    private void UpdateCheckPointUI()
    {
        checkPointsUI.text = 
            raceSettingsData.CheckPointsCollected + " / " + raceSettingsData.CheckPointsAmount;
    }

    private void StructuresPlacement()
    {
        poissonDiskData.Init(Seed);
        System.Random prgn = new System.Random(Seed);

        if (poissonDiskData.PoissonDiscPoints != null)
        {
            foreach (var point in poissonDiskData.PoissonDiscPoints)
            {
                var vertex = vertexMapData[Mathf.FloorToInt(point.x), Mathf.FloorToInt(point.y)];
                var biome = vertex.biomeList[prgn.Next(0, vertex.biomeList.Count)];
                var structure = biome.structures[prgn.Next(0, biome.structures.Length)];

                Instantiate(structure.structure, new Vector3(point.x, vertex.height * perlinData.HeightMultiplier, point.y), Quaternion.identity, poissonDiskData.StructureParent);
            }
        }
    }

    private void AssignValuesToVertex()
    {
        var count = 0;

        for (int y = 0; y < mapData.MapSize.y; y++)
        {
            for (int x = 0; x < mapData.MapSize.x; x++)
            {
                var pVertex = perlinData.NoiseMap[x, y];
                var vVertex = voronoiData.VoronoiMap[x, y];

                vertexMapData[x, y] = new Vertex()
                {
                    height = pVertex.height,
                    friction = vVertex.friction,
                    color = vVertex.color,
                    biomeList = vVertex.biomeList,
                };

                count++;
                _averageMapHeight = _averageMapHeight * (count - 1) / count + pVertex.height / count;
                _averageMapFriction = _averageMapFriction * (count - 1) / count + vVertex.friction / count;
            }
        }
    }

    public void GenerateMesh()
    {
        Mesh mesh = MeshGenerator.GenerateTerrainMesh(
            vertexMapData, perlinData.HeightMultiplier, mapData.FlatShading);
        mapData.MeshFilter.sharedMesh = mesh;
        mapData.MeshCollider.sharedMesh = mesh;
    }

    public void DisplayMap() 
    {
        Texture2D texture = new Texture2D(mapData.MapSize.x, mapData.MapSize.y);

        switch (mapData.DisplayMode)
        {
            case DisplayMode.NoiseMap:
                texture.SetPixels32(MapDisplay.DrawNoiseMap(vertexMapData));
                break;

            case DisplayMode.VoronoiMap:
                texture.SetPixels32(MapDisplay.DrawVoronoiMap(vertexMapData));
                break;
        }

        texture.filterMode = FilterMode.Point;
        texture.Apply();

        mapData.MeshRenderer.sharedMaterial.mainTexture = texture;
    }
}