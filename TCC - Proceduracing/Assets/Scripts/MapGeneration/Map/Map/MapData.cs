using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapData: MonoBehaviour
{
    [Header("General Settings")]
    [SerializeField] private GameObject terrain;
    [SerializeField] private Vector2Int mapSize;
    [SerializeField] private Vector2 offset;
    [SerializeField] private DisplayMode displayMode;
    [SerializeField] bool flatShading;

    private MeshFilter meshFilter;
    private MeshCollider meshCollider;
    private Renderer meshRenderer;

    public MeshFilter MeshFilter { get => meshFilter; private set => meshFilter = value; }
    public MeshCollider MeshCollider { get => meshCollider; private set => meshCollider = value; }
    public Renderer MeshRenderer { get => meshRenderer; private set => meshRenderer = value; }
    public Vector2Int MapSize { get => mapSize; private set => MapSize = value; }
    public Vector2 Offset { get => offset; private set => offset = value; }
    internal DisplayMode DisplayMode { get => displayMode; private set => displayMode = value; }
    public bool FlatShading { get => flatShading; private set => flatShading = value; }

    private void Awake()
    {
        void GetTerrainMeshComponents()
        {
            terrain.TryGetComponent(out MeshFilter meshFilter);
                this.meshFilter = meshFilter;

            terrain.TryGetComponent(out MeshCollider meshCollider);
                this.meshCollider = meshCollider;

            terrain.TryGetComponent(out MeshRenderer meshRenderer);
                this.meshRenderer = meshRenderer;
        }

        GetTerrainMeshComponents();
    }

    public Vertex[,] GetVertexMap()
    { 
        return new Vertex[255, 255];
    }

}
enum DisplayMode
{
    NoiseMap,
    VoronoiMap
}