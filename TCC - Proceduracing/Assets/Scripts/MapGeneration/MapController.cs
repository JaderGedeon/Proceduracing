using System;
using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] private Vector2 offset;
    [SerializeField] private DisplayMode displayMode;
    [SerializeField] bool flatShading;

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
        noiseMap = PerlinNoise.GenerateNoiseMap(mapSize, seed, noiseScale, octaves, persistence, lacunarity, offset);
        voronoiMap = VoronoiNoise.GenerateNoiseMap(mapSize, seed, regionAmount, regionMinimumInfluence);
        GenerateMesh();
        DisplayMap();
    }

    public void GenerateMesh()
    {
        Mesh mesh = MeshGenerator.GenerateTerrainMesh(noiseMap, heightMultiplier, flatShading);
        meshFilter.sharedMesh = mesh;
        meshCollider.sharedMesh = mesh;
    }

    public void DisplayMap() 
    {
        Texture2D texture = new Texture2D(mapSize.x, mapSize.y);

        switch (displayMode)
        {
            case DisplayMode.NoiseMap:
                texture.SetPixels32(MapDisplay.DrawNoiseMap(noiseMap));
                break;

            case DisplayMode.VoronoiMap:
                texture.SetPixels32(MapDisplay.DrawVoronoiMap(voronoiMap));
                break;
        }

        texture.filterMode = FilterMode.Point;
        texture.Apply();

        meshRenderer.sharedMaterial.mainTexture = texture;
    }
}
