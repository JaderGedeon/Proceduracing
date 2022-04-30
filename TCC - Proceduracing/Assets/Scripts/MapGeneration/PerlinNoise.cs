using System.Collections;
using UnityEngine;

public class PerlinNoise
{
    public static Vertex[,] GenerateNoiseMap(Vector2Int mapSize, int seed, float scale, int octaves, float persistence, float lacunarity, Vector2 offset) {

        Vertex[,] noiseMap = new Vertex[mapSize.x, mapSize.y];

        System.Random prgn = new System.Random(seed);
        Vector2[] octaveOffsets = new Vector2[octaves];
        for (int i = 0; i < octaves; i++)
        {
            float offsetX = prgn.Next(-100000, 100000) + offset.x;
            float offsetY = prgn.Next(-100000, 100000) - offset.y;
            octaveOffsets[i] = new Vector2(offsetX, offsetY);
        }

        if (scale <= 0)
            scale = 0.0001f;

        float maxNoiseHeight = float.MinValue;
        float minNoiseHeight = float.MaxValue;

        float halfWidth = mapSize.x / 2f;
        float halfHeight = mapSize.y / 2f;

        for (int y = 0; y < mapSize.y; y++)
        {
            for (int x = 0; x < mapSize.x; x++)
            {

                float amplitude = 1;
                float frequency = 1;
                float noiseHeight = 0;

                for (int i = 0; i < octaves; i++)
                {
                    float sampleX = (x-halfWidth) / scale * frequency - octaveOffsets[i].x * frequency;
                    float sampleY = (y-halfHeight) / scale * frequency - octaveOffsets[i].y * frequency;

                    float perlinValue = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1;
                    noiseHeight += perlinValue * amplitude;

                    amplitude *= persistence;
                    frequency *= lacunarity;
                }

                if (noiseHeight > maxNoiseHeight)
                {
                    maxNoiseHeight = noiseHeight;
                }
                else if (noiseHeight < minNoiseHeight)
                {
                    minNoiseHeight = noiseHeight;
                }

                noiseMap[x, y] = new Vertex(noiseHeight);
            }
        }

        for (int y = 0; y < mapSize.y; y++)
        {
            for (int x = 0; x < mapSize.x; x++)
            {
                noiseMap[x, y].height = Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, noiseMap[x, y].height);
            }
        }

        return noiseMap;

    }
}
