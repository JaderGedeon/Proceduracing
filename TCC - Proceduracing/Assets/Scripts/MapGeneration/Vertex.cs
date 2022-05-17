using System.Collections;
using UnityEngine;

public class Vertex
{
    public float height;
    public BiomeScriptableObject biome;

    public override string ToString()
    {
        return ("Height: "+height);
    }
}