using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoronoiData : MonoBehaviour
{
    [Header("Voronoi Settings")]
    [Range(1, 2)]
    [SerializeField] private int regionAmount;
    [SerializeField] private Vector2Int regionMinMaxRadius;
    [Range(0, 1f)]
    [SerializeField] private float regionTransition;

    private VoronoiNoise voronoiNoise;

    public Vertex[,] VoronoiMap => voronoiNoise.VoronoiMap;

    public void Init(Vector2Int mapSize, Vertex[,] noiseMap, MinMax minMax, int seed)
    {
        voronoiNoise = new VoronoiNoise(regionAmount, regionMinMaxRadius, regionTransition);
        voronoiNoise.GenerateVoronoiMap(mapSize, noiseMap, minMax, seed);
    }
}
