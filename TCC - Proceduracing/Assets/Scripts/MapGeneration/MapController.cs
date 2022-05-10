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
    [SerializeField] private Vector2Int mapSize;
    [SerializeField] private int seed;
    [SerializeField] private Vector2 offset;
    [SerializeField] DisplayMode displayMode;

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
    [SerializeField] private int regionColorAmount;
    [Range(0f, 1f)]
    [SerializeField] private float regionMinimumInfluence;

    [SerializeField] private Vector2[] points;


    [SerializeField] private MapDisplay mapDisplay;
    [SerializeField] private MeshGenerator meshGenerator;

    private void Start()
    {
        GenerateMap();
    }

    public void GenerateMap()
    {
        noiseMap = PerlinNoise.GenerateNoiseMap(mapSize, seed, noiseScale, octaves, persistence, lacunarity, offset);

        noiseMap[0, 0].height = 100;
        noiseMap[1, 0].height = 100;
        noiseMap[2, 0].height = 100;
        GenerateMesh();

        //

        points = new Vector2[regionAmount];
        Color[] regionColors = new Color[regionColorAmount];

        System.Random prgn = new System.Random(seed);

        for (int i = 0; i < regionAmount; i++)
        {
            points[i] = new Vector2(prgn.Next(0, mapSize.x), prgn.Next(0, mapSize.y));
        }

        for (int i = 0; i < regionColorAmount; i++)
        {
            regionColors[i] = new Color(prgn.Next(0, 255) / 255f, prgn.Next(0, 255) / 255f, prgn.Next(0, 255) / 255f);
        }

        Color[] pixelColor = new Color[mapSize.x * mapSize.y];

        for (int y = 0; y < mapSize.y - 1; y++)
        {
            for (int x = 0; x < mapSize.x - 1; x++)
            {
                float[] regionDistances = new float[regionAmount];
                float sum = 0;

                float[] newRegionDistances = new float[regionAmount];
                float newSum = 0f;

                for (int i = 0; i < regionAmount; i++)
                {
                    regionDistances[i] = Vector2.Distance(new Vector2(y, (mapSize.x - 1 - x)), points[i]);
                    sum += regionDistances[i];
                }

                for (int i = 0; i < regionAmount; i++)
                {
                    newRegionDistances[i] = sum - regionDistances[i];
                    newSum += newRegionDistances[i];
                }

                Color gradiantPixelColor = new Color();

                var value = 0;
                var maxInfluence = 0f;

                for (int i = 0; i < regionAmount; i++)
                {
                    var regionInfluence = (newRegionDistances[i] / newSum);

                    if (regionInfluence <= regionMinimumInfluence)
                    {
                        gradiantPixelColor.r += regionColors[i].r * regionInfluence;
                        gradiantPixelColor.g += regionColors[i].g * regionInfluence;
                        gradiantPixelColor.b += regionColors[i].b * regionInfluence;
                    }
                    else {
                        if (regionInfluence > maxInfluence) {
                            maxInfluence = regionInfluence;
                            value = i;
                        }
                    }
                }

                if (maxInfluence != 0) 
                {
                    gradiantPixelColor.r = regionColors[value].r;
                    gradiantPixelColor.g = regionColors[value].g;
                    gradiantPixelColor.b = regionColors[value].b;
                }

                pixelColor[(y + x * mapSize.y)] = gradiantPixelColor;
            }
        }
        //

        switch (displayMode) {
            case DisplayMode.NoiseMap:
                mapDisplay.DrawNoiseMap(noiseMap);
                break;
                
            case DisplayMode.VoronoiMap:
                mapDisplay.DrawVoronoiMap(pixelColor, mapSize);
                break;
        }
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

    private void OnDrawGizmos()
    {
        for (int i = 0; i < points.Length; i++)
        {
            Gizmos.color = new Color(0, 0, 1, 0.7f);
            Gizmos.DrawSphere(new Vector3(points[i].x - mapSize.x / 2, 5, points[i].y - mapSize.y / 2), 10f);
        }

        Vector2Int noiseSize = new Vector2Int(noiseMap.GetLength(0), noiseMap.GetLength(1));

        for (int y = 0; y < noiseSize.y / 10; y++)
        {
            for (int x = 0; x < noiseSize.x / 10; x++)
            {
                Gizmos.color = new Color(1, 0, 1, 0.8f);
                if (x == 0)
                    Gizmos.color = new Color(1, 1, 0, 0.8f);

                if (y == 0)
                    Gizmos.color = new Color(0, 1, 1, 0.8f);

                Gizmos.DrawSphere(new Vector3(x - mapSize.x / 2, noiseMap[x,y].height * heightMultiplier, y - mapSize.y / 2), 0.1f);
            }
        }
    }
}
