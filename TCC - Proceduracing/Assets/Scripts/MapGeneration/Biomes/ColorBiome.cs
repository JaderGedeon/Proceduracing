using UnityEngine;

[System.Serializable]
public struct ColorBiome
{
    public Color color;
    public float minHeight;
    public float maxHeight;

    public ColorBiome(Color color, float minHeight, float maxHeight)
    {
        this.color = color;
        this.minHeight = minHeight;
        this.maxHeight = maxHeight;
    }
}
