using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{
    [SerializeField] private Vector2Int mapSize;
    [SerializeField] private float noiseScale;
    [SerializeField] private float heightMultiplier;
    [SerializeField] private int octaves;
    [Range(0,1)]
    [SerializeField] private float persistence;
    [SerializeField] private float lacunarity;
    [SerializeField] private int seed;
    [SerializeField] private Vector2 offset;

    private Vertex[,] noiseMap;

    [SerializeField] private MapDisplay mapDisplay;
    [SerializeField] private MeshGenerator meshGenerator;

    private void Start()
    {
        GenerateMap();
    }

    public void GenerateMap()
    {
        noiseMap = PerlinNoise.GenerateNoiseMap(mapSize, seed, noiseScale, octaves, persistence, lacunarity, offset);
        GenerateMesh();
        mapDisplay.DrawNoiseMap(noiseMap);
    }

    public void GenerateMesh()
    {
       meshGenerator.Generate(mapSize, noiseMap, heightMultiplier);
    }

    private void OnValidate()
    {
        if (mapSize.x < 2)
            mapSize.x = 2;

        if (mapSize.y < 2)
            mapSize.y = 2;

        if (lacunarity < 1)
            lacunarity = 1;

        if (octaves < 0)
            octaves = 0;
    }
}
