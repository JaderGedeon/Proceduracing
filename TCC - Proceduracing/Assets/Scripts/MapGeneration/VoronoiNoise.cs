using System.Linq;
using System.Collections;
using System;
using UnityEngine;
using System.Collections.Generic;

public class VoronoiNoise : MonoBehaviour
{
    public static Vertex[,] GenerateNoiseMap(Vector2Int mapSize, int seed, int regionAmount, float regionMinimumInfluence, int maxCentralArea, Vector2Int minMaxRadius, float biomeTransition)
    {
        Vertex[,] vertexMap = new Vertex[mapSize.x, mapSize.y];
        SubRegion[] regions = new SubRegion[regionAmount];
        Vector2Int centralPoint = new Vector2Int(mapSize.x / 2, mapSize.y / 2);

        // OTIMIZAR ESSA LINHA     
        List<BiomeScriptableObject> biomeList = Resources.LoadAll<BiomeScriptableObject>("Data/ScriptableObjects/Biomes").ToList();

        Region baseRegion = new Region(null);

        System.Random prgn = new System.Random(seed);

        List<Vector2> biomePoints = PoissonDiscSampling.GeneratePoints(minMaxRadius.y, seed, mapSize, 200, regionAmount);

        for (int i = -1; i < regionAmount; i++)
        {
            BiomeScriptableObject biome = biomeList[prgn.Next(0, biomeList.Count)];
            float radius = prgn.Next(minMaxRadius.x, minMaxRadius.y);

            if (i == -1)
                baseRegion = new Region(biome);
            else
                regions[i] = new SubRegion(biomePoints[i], biome, radius);
        }

        for (int y = 0; y < mapSize.y; y++)
        {
            for (int x = 0; x < mapSize.x; x++)
            {
                RegionDistance[] regionDist = new RegionDistance[regionAmount];
                bool outsideSubRegion = false;

                for (int i = 0; i < regionAmount; i++)
                {
                    var distance = Vector2.Distance(new Vector2(x, y), regions[i].centerPosition);

                    if (distance < regions[i].radius)
                        outsideSubRegion = true;

                    regionDist[i] = new RegionDistance(i, distance);
                }

                VertexBiomeInfo vertexBiomeInfo = new VertexBiomeInfo();

                if (!outsideSubRegion)
                {
                    vertexBiomeInfo.friction = baseRegion.biome.friction;
                    vertexBiomeInfo.color = baseRegion.biome.gradient.colorKeys[0].color;
                }
                else {

                    float sumDistances = regionDist.Sum(element => element.distance);
                    Array.ForEach(regionDist, r => r.CalculateInfluence(sumDistances, regionAmount));

                    regionDist = (from r in regionDist orderby r.distance select r).ToArray();

                    for (int j = 0; j < regionAmount; j++)
                    {
                        if (j != regionAmount - 1)
                        {
                            if (regionDist[j].influence - regionDist[j + 1].influence > regionMinimumInfluence)
                            {
                                vertexBiomeInfo.friction = regions[regionDist[j].index].biome.friction;
                                vertexBiomeInfo.color = regions[regionDist[j].index].biome.gradient.colorKeys[0].color;
                                break;
                            }
                        }
                        vertexBiomeInfo.friction += regions[regionDist[j].index].biome.friction * regionDist[j].influence;
                        vertexBiomeInfo.color += regions[regionDist[j].index].biome.gradient.colorKeys[0].color * regionDist[j].influence;
                    }
                }

                vertexMap[x, y] = new Vertex
                {
                    friction = vertexBiomeInfo.friction,
                    color = vertexBiomeInfo.color,
                };
            }
        }
        return vertexMap;
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

        public SubRegion(Vector2 centerPosition, BiomeScriptableObject biome, float radius) : base(biome)
        {
            this.centerPosition = centerPosition;
            this.biome = biome;
            this.radius = radius;
        }
    }

    private class RegionDistance
    {
        public int index;
        public float distance;
        public float influence;

        public RegionDistance(int index, float distance)
        {
            this.index = index;
            this.distance = distance;
        }

        public void CalculateInfluence(float sum, int regionAmount)
        {
            influence = (sum - distance) / sum / (regionAmount - 1);
        }
    }

    private class VertexBiomeInfo
    {
        public float friction;
        public Color32 color;
    }
}