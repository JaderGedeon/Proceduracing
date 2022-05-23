using System.Linq;
using System.Collections;
using System;
using UnityEngine;
using System.Collections.Generic;

public class VoronoiNoise : MonoBehaviour
{
    public static Vertex[,] GenerateNoiseMap(Vector2Int mapSize, int seed, int regionAmount, Vector2Int minMaxRadius, float biomeTransition)
    {
        Vertex[,] vertexMap = new Vertex[mapSize.x, mapSize.y];
        Vector2Int centralPoint = new Vector2Int(mapSize.x / 2, mapSize.y / 2);
        System.Random prgn = new System.Random(seed);

        // OTIMIZAR ESSA LINHA     
        List<BiomeScriptableObject> biomeList = Resources.LoadAll<BiomeScriptableObject>("Data/ScriptableObjects/Biomes").ToList();
        biomeList = biomeList.OrderBy(b => prgn.Next()).ToList();
        List<Vector2> biomePoints = PoissonDiscSampling.GeneratePoints(minMaxRadius.y, seed, mapSize, 9999, regionAmount - 1);

        Region baseRegion = new Region(biomeList[0]);
        SubRegion subRegion = new SubRegion(biomePoints[0], biomeList[1], prgn.Next(minMaxRadius.x, minMaxRadius.y), biomeTransition);

        for (int y = 0; y < mapSize.y; y++)
        {
            for (int x = 0; x < mapSize.x; x++)
            {
                VertexBiomeInfo vertexBiomeInfo = new VertexBiomeInfo();
                vertexBiomeInfo.SubstituteValue(baseRegion);

                if (regionAmount > 1)
                {
                    float distance = Vector2.Distance(new Vector2(x, y), subRegion.centerPosition);

                    if (distance <= subRegion.radius)
                    {
                        vertexBiomeInfo = new VertexBiomeInfo();

                        if (distance <= subRegion.middleRadius)
                        {
                            vertexBiomeInfo.SubstituteValue(subRegion);
                        }
                        else
                        {
                            float gradientValue = 1 - ((distance - subRegion.middleRadius) / (subRegion.radius - subRegion.middleRadius));
                            vertexBiomeInfo.SumValues(baseRegion, subRegion, gradientValue);
                        }
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
        public float middleRadius;

        public SubRegion(Vector2 centerPosition, BiomeScriptableObject biome, float radius, float biomeTransition) : base(biome)
        {
            this.centerPosition = centerPosition;
            this.biome = biome;
            this.radius = radius;
            this.middleRadius = radius * (1 - biomeTransition);
        }
    }

    private class RegionDistance
    {
        public float distance;
        public float influence;

        public RegionDistance(float distance)
        {
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
        public Color color;

        public void SumValues(Region baseRegion, Region subRegion, float multiplier)
        {
            this.friction += baseRegion.biome.friction * (1 - multiplier);
            this.color += baseRegion.biome.gradient.colorKeys[0].color * (1 - multiplier);

            this.friction += subRegion.biome.friction * multiplier;
            this.color += subRegion.biome.gradient.colorKeys[0].color * multiplier;
        }

        public void SubstituteValue(Region region)
        {
            this.friction = region.biome.friction;
            this.color = region.biome.gradient.colorKeys[0].color;
        }
    }
}


// Arrumar aqui

/*
for (int j = 0; j < regionAmount - 1; j++)
{
    if (j != regionAmount - 1)
    {
        if (regionAmount == 2 || regionDist[j].influence - regionDist[j + 1].influence > regionMinimumInfluence)
        {
            vertexBiomeInfo.SubstituteValue(regions[regionDist[j].index]);
            break;
        }
    }
    vertexBiomeInfo.SumValues(regions[regionDist[j].index], regionDist[j].influence);
}  */

// Arrumar aqui