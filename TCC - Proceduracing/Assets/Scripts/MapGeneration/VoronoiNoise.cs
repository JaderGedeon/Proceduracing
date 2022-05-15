using System.Collections;
using UnityEngine;

public class VoronoiNoise : MonoBehaviour
{
    public static Vertex[,] GenerateNoiseMap(Vector2Int mapSize, int seed, int regionAmount, float regionMinimumInfluence) {

        Vertex[,] noiseMap = new Vertex[mapSize.x, mapSize.y];
        Vector2[] regions = new Vector2[regionAmount];
        Color[] regionColors = new Color[regionAmount];

        System.Random prgn = new System.Random(seed);

        for (int i = 0; i < regionAmount; i++)
        {
            regions[i] = new Vector2(prgn.Next(0, mapSize.x), prgn.Next(0, mapSize.y));
            regionColors[i] = new Color(prgn.Next(0, 255) / 255f, prgn.Next(0, 255) / 255f, prgn.Next(0, 255) / 255f);
        }

        for (int y = 0; y < mapSize.y; y++)
        {
            for (int x = 0; x < mapSize.x; x++)
            {
                float[] regionDistances = new float[regionAmount];
                float sum = 0;

                float[] newRegionDistances = new float[regionAmount];
                float newSum = 0f;

                for (int i = 0; i < regionAmount; i++)
                {
                    regionDistances[i] = Vector2.Distance(new Vector2(y, (mapSize.x - 1 - x)), regions[i]);
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
                        gradiantPixelColor.r += regionColors[i].r * regionInfluence;
                        gradiantPixelColor.g += regionColors[i].g * regionInfluence;
                        gradiantPixelColor.b += regionColors[i].b * regionInfluence;
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
                    gradiantPixelColor.r = regionColors[value].r;
                    gradiantPixelColor.g = regionColors[value].g;
                    gradiantPixelColor.b = regionColors[value].b;
                }

                noiseMap[x, y] = new Vertex(gradiantPixelColor);
            }
        }

        return noiseMap;
    }  
}
