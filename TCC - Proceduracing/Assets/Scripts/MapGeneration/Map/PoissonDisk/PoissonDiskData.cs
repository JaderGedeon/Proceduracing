using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoissonDiskData : MonoBehaviour
{
    [Header("Poisson Disk Settings")]

    [SerializeField] private float radius = 1;
    [SerializeField] private Vector2 regionSize = Vector2.one;
    [SerializeField] private int rejectionSamples = 30;
    [SerializeField] private float displayRadius = 1;
    [SerializeField] private Transform structureParent;

    private PoissonDiscSampling poissonDiscSampling;

    public PoissonDiskData(float radius, Vector2 regionSize)
    {
        this.radius = radius;
        this.regionSize = regionSize;
    }

    public List<Vector2> PoissonDiscPoints => poissonDiscSampling.PoissonDiscPoints;

    public Transform StructureParent { get => structureParent; private set => structureParent = value; }

    public void Init(int seed)
    {
        poissonDiscSampling = new PoissonDiscSampling(radius, seed, regionSize);
        poissonDiscSampling.GeneratePoints();
    }

    public void Init(int seed, int biomePoisson)
    {
        poissonDiscSampling = new PoissonDiscSampling(radius, seed, regionSize);
        poissonDiscSampling.GeneratePoints(biomePoisson);
    }
}
