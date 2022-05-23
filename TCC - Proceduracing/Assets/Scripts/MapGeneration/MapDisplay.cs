using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapDisplay : MonoBehaviour
{
    public static Color32[] DrawNoiseMap(Vertex[,] noiseMap)
    {
        Vector2Int mapSize = new Vector2Int(noiseMap.GetLength(0), noiseMap.GetLength(1));

        Color32[] colourMap = new Color32[mapSize.x * mapSize.y];
        for (int y = 0; y < mapSize.y; y++)
        {
            for (int x = 0; x < mapSize.x; x++)
            {
                colourMap[(y * mapSize.x) + x] = Color32.Lerp(Color.white, Color.black, noiseMap[x, y].height);
            }
        }
        return colourMap;
    }

    public static Color32[] DrawVoronoiMap(Vertex[,] voronoiMap) {

        Vector2Int mapSize = new Vector2Int(voronoiMap.GetLength(0), voronoiMap.GetLength(1));

        Color32[] colourMap = new Color32[mapSize.x * mapSize.y];
        for (int y = 0; y < mapSize.y; y++)
        {
            for (int x = 0; x < mapSize.x; x++)
            {
                colourMap[y * mapSize.x + x] = voronoiMap[x, y].color;
            }
        }
        return colourMap;
    }
}