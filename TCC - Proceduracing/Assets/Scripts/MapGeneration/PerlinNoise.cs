using System;
using System.Collections;
using UnityEngine;

public class PerlinNoise
{
    private float _scale;
    private int _octaves;
    private float _persistence;
    private float _lacunarity;

    public MinMax MinMax { get; private set; }
    public Vertex[,] NoiseMap { get; private set; }

    public PerlinNoise(float scale, int octaves, float persistence, float lacunarity)
    {
        _scale = scale > 0 ? scale : 0.0001f;
        _octaves = octaves;
        _persistence = persistence;
        _lacunarity = lacunarity;
    }

    public void GenerateNoiseMap(Vector2Int mapSize, int seed, Vector2 offset)
    {
        NoiseMap = new Vertex[mapSize.x, mapSize.y];
        MinMax = new MinMax();

        System.Random prgn = new System.Random(seed);
        Vector2[] octaveOffsets = new Vector2[_octaves];
        for (int i = 0; i < _octaves; i++)
        {
            float offsetX = prgn.Next(-100000, 100000) + offset.x;
            float offsetY = prgn.Next(-100000, 100000) - offset.y;
            octaveOffsets[i] = new Vector2(offsetX, offsetY);
        }

        for (int y = 0; y < mapSize.y; y++)
        {
            for (int x = 0; x < mapSize.x; x++)
            {
                float amplitude = 1;
                float frequency = 1;
                float noiseHeight = 0;

                for (int i = 0; i < _octaves; i++)
                {
                    float sampleX = x / _scale * frequency - octaveOffsets[i].x * frequency;
                    float sampleY = y / _scale * frequency - octaveOffsets[i].y * frequency;

                    float perlinValue = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1;
                    noiseHeight += perlinValue * amplitude;

                    amplitude *= _persistence;
                    frequency *= _lacunarity;
                }

                MinMax.AddValue(noiseHeight);

                NoiseMap[x, y] = new Vertex()
                {
                    height = noiseHeight
                };
            }
        }

        for (int y = 0; y < mapSize.y; y++)
        {
            for (int x = 0; x < mapSize.x; x++)
            {
                NoiseMap[x, y].height = Mathf.InverseLerp(MinMax.Min, MinMax.Max, NoiseMap[x, y].height);
            }
        }
    }
}
