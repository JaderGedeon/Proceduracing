using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapDisplay : MonoBehaviour
{
    public Renderer textureRenderer;

    public void DrawNoiseMap(Vertex[,] noiseMap)
    {
        Vector2Int mapSize = new Vector2Int(noiseMap.GetLength(0), noiseMap.GetLength(1));

        Texture2D texture = new Texture2D(mapSize.x, mapSize.y);

        Color[] colourMap = new Color[mapSize.x * mapSize.y];
        for (int y = 0; y < mapSize.y - 1; y++)
        {
            for (int x = 0; x < mapSize.x - 1; x++)
            {
                colourMap[(y * mapSize.x) + x] = Color.Lerp(Color.white, Color.black, noiseMap[x, y].height);
            }
        }
        texture.SetPixels(colourMap);
        texture.filterMode = FilterMode.Point;
        texture.Apply();

        textureRenderer.sharedMaterial.mainTexture = texture;

    }
}