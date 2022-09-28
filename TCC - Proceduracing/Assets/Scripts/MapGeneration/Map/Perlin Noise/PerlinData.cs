using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerlinData : MonoBehaviour
{
    [Header("Perlin Settings")]
    [SerializeField] private float noiseScale;
    [SerializeField] private float heightMultiplier;
    [SerializeField] private int octaves;
    [Range(0, 1)]
    [SerializeField] private float persistence;
    [SerializeField] private float lacunarity;

    private PerlinNoise perlinNoise;

    public Vertex[,] NoiseMap => perlinNoise.NoiseMap;

    public MinMax MinMax => perlinNoise.MinMax;

    public float HeightMultiplier { get => heightMultiplier; private set => heightMultiplier = value; }

    public void Init(Vector2Int mapSize, int seed, Vector2 offset)
    {
        perlinNoise = new PerlinNoise(noiseScale, octaves, persistence, lacunarity);
        perlinNoise.GenerateNoiseMap(mapSize, seed, offset);
    }
}
