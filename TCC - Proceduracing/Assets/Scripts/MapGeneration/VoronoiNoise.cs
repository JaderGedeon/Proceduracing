using System.Linq;
using System.Collections;
using System;
using UnityEngine;
using System.Collections.Generic;

public class VoronoiNoise : MonoBehaviour
{
    public static Vertex[,] GenerateNoiseMap(Vector2Int mapSize, int seed, int regionAmount, float regionMinimumInfluence) {

        Vertex[,] vertexMap = new Vertex[mapSize.x, mapSize.y];
        Region[] regions = new Region[regionAmount];

        // OTIMIZAR ESSA LINHA     
        List<BiomeScriptableObject> biomeList = Resources.LoadAll<BiomeScriptableObject>("Data/ScriptableObjects/Biomes").ToList();
        Debug.Log(biomeList.Count);

        System.Random prgn = new System.Random(seed);

        for (int i = 0; i < regionAmount; i++)
        {
            regions[i] = new Region(new Vector2(prgn.Next(0, mapSize.x), prgn.Next(0, mapSize.y)), biomeList[prgn.Next(0, biomeList.Count)]);
            Debug.Log("X:" + (regions[i].CenterPosition.x) + " | Y: "+(regions[i].CenterPosition.y) + " | Color: " + regions[i].Biome.colors[0].color);
        }

        for (int y = 0; y < mapSize.y; y++)
        {
            for (int x = 0; x < mapSize.x; x++)
            {
                int nearRegionValue = 0;
                float nearRegionDistance = Mathf.Infinity;

                for (int i = 0; i < regionAmount; i++)
                {
                    var distance = Vector2.Distance(new Vector2(x, y), regions[i].CenterPosition);

                    if (nearRegionDistance >= distance)
                    {
                        nearRegionDistance = distance;
                        nearRegionValue = i;
                    }
                }

                vertexMap[x, y] = new Vertex()
                {
                    biome = regions[nearRegionValue].Biome,
                };

                /*
                
                float[] regionDistances = new float[regionAmount];
                float sum = 0;

                float[] newRegionDistances = new float[regionAmount];
                float newSum = 0f;

                for (int i = 0; i < regionAmount; i++)
                {
                    regionDistances[i] = Vector2.Distance(new Vector2(y, (mapSize.x - 1 - x)), regions[i].CenterPosition);
                    sum += regionDistances[i];
                }

                for (int i = 0; i < regionAmount; i++)
                {
                    newRegionDistances[i] = sum - regionDistances[i];
                    newSum += newRegionDistances[i] * regionMinimumInfluence;
                }

                Color gradiantPixelColor = new Color();

                var value = 0;
                var maxInfluence = -1f;

                for (int i = 0; i < regionAmount; i++)
                {
                    var regionInfluence = (newRegionDistances[i] / newSum);

                    if (regionInfluence <= regionMinimumInfluence)
                    {
                        gradiantPixelColor.r += regions[i].Biome.colors[0].color.r;
                        gradiantPixelColor.g += regions[i].Biome.colors[0].color.g;
                        gradiantPixelColor.b += regions[i].Biome.colors[0].color.b;
                    }
                    else
                    {
                        if (regionInfluence > maxInfluence)
                        {
                            maxInfluence = regionInfluence;
                            value = i;
                        }
                    }
                }

                if (maxInfluence != -1f)
                {
                    gradiantPixelColor = regions[value].Biome.colors[0].color;
                }

                vertexMap[x, y] = new Vertex()
                {
                    colour = gradiantPixelColor
                };
            }
            */
            }
        }
        return vertexMap;
    }
}

public class Region
{
    private Vector2 centerPosition;
    private BiomeScriptableObject biome;

    public Region(Vector2 centerPosition, BiomeScriptableObject biome)
    {
        this.centerPosition = centerPosition;
        this.biome = biome;
    }

    public Vector2 CenterPosition { get => centerPosition; set => centerPosition = value; }
    public BiomeScriptableObject Biome { get => biome; set => biome = value; }
}