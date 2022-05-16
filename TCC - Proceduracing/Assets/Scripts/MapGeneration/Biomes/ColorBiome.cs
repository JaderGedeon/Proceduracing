using UnityEngine;

[System.Serializable]
public struct ColorBiome
{
    public Color32 color;
    public float minHeight;
    public float maxHeight;

    public ColorBiome(Color32 color, float minHeight, float maxHeight)
    {
        this.color = color;
        this.minHeight = minHeight;
        this.maxHeight = maxHeight;
    }
}
