using System.Linq;
using System.Collections;
using System;
using UnityEngine;
using System.Collections.Generic;

public class VoronoiNoise
{
    private int _regionAmount;
    private Vector2Int _regionMinMaxRadius;   
    private float _regionTransition;

    public VoronoiNoise(int regionAmount, Vector2Int regionMinMaxRadius, float regionTransition)
    {
        _regionAmount = regionAmount;
        _regionMinMaxRadius = regionMinMaxRadius;
        _regionTransition = regionTransition;
    }

    public Vertex[,] VoronoiMap { get; private set; }

    public void GenerateVoronoiMap(Vector2Int mapSize, Vertex[,] noiseMap, MinMax minMax, int seed)
    {
        VoronoiMap = new Vertex[mapSize.x, mapSize.y];

        Vector2Int centralPoint = new Vector2Int(mapSize.x / 2, mapSize.y / 2);
        System.Random prgn = new System.Random(seed);

        // OTIMIZAR ESSA LINHA     
        List<BiomeScriptableObject> biomeList = Resources.LoadAll<BiomeScriptableObject>("Data/ScriptableObjects/Biomes").ToList();
        biomeList = biomeList.OrderBy(b => prgn.Next()).ToList();

        var poissonDisk = new PoissonDiskData(_regionMinMaxRadius.y, mapSize);
        poissonDisk.Init(seed, _regionAmount - 1);

        Region baseRegion = new Region(biomeList[0]);

        SubRegion subRegion = new SubRegion(poissonDisk.PoissonDiscPoints[0], biomeList[1], prgn.Next(_regionMinMaxRadius.x, _regionMinMaxRadius.y), _regionTransition);

        for (int y = 0; y < mapSize.y; y++)
        {
            for (int x = 0; x < mapSize.x; x++)
            {
                float gradientTime = (noiseMap[x, y].height - minMax.Min) / (minMax.Max - minMax.Min);

                VertexBiomeInfo vertexBiomeInfo = new VertexBiomeInfo();
                vertexBiomeInfo.SubstituteValue(baseRegion, gradientTime);

                if (_regionAmount > 1)
                {
                    float distance = Vector2.Distance(new Vector2(x, y), subRegion.centerPosition);

                    if (distance <= subRegion.radius)
                    {
                        vertexBiomeInfo = new VertexBiomeInfo();

                        if (distance <= subRegion.middleRadius)
                        {
                            vertexBiomeInfo.SubstituteValue(subRegion, gradientTime);
                        }
                        else
                        {
                            float gradientValue = 1 - ((distance - subRegion.middleRadius) / (subRegion.radius - subRegion.middleRadius));
                            vertexBiomeInfo.SumValues(baseRegion, subRegion, gradientValue, gradientTime);
                        }
                    }
                }

                VoronoiMap[x, y] = new Vertex
                {
                    friction = vertexBiomeInfo.friction,
                    color = vertexBiomeInfo.color,
                    biomeList = vertexBiomeInfo.biomeList,
                };
            }
        }

        Debug.Log("aabc");
    }

    private class Region
    {
        public BiomeScriptableObject biome;

        public Region(BiomeScriptableObject biome)
        {
            this.biome = biome;
        }
    }

    private class SubRegion : Region
    {
        public Vector2 centerPosition;
        public float radius;
        public float middleRadius;

        public SubRegion(Vector2 centerPosition, BiomeScriptableObject biome, float radius, float biomeTransition) : base(biome)
        {
            this.centerPosition = centerPosition;
            this.biome = biome;
            this.radius = radius;
            this.middleRadius = radius * (1 - biomeTransition);
        }
    }

    private class VertexBiomeInfo
    {
        public float friction;
        public Color color;
        public List<BiomeScriptableObject> biomeList;

        public VertexBiomeInfo()
        {
            biomeList = new List<BiomeScriptableObject>();
        }

        public void SumValues(Region baseRegion, Region subRegion, float multiplier, float gradientTime)
        {
            this.friction += baseRegion.biome.friction * (1 - multiplier);
            this.color += baseRegion.biome.gradient.Evaluate(gradientTime) * (1 - multiplier);
            this.biomeList.Add(baseRegion.biome);

            this.friction += subRegion.biome.friction * multiplier;
            this.color += subRegion.biome.gradient.Evaluate(gradientTime) * multiplier;
            this.biomeList.Add(subRegion.biome);
        }

        public void SubstituteValue(Region region, float gradientTime)
        {
            this.friction = region.biome.friction;
            this.color = region.biome.gradient.Evaluate(gradientTime);
            this.biomeList.Clear();
            this.biomeList.Add(region.biome);
        }
    }
}