using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoissonDiscSampling
{
	private float _radius;
	private int _seed;
	private Vector2 _sampleRegionSize;
	private int _numSamplesBeforeRejection;

    private System.Random prgn;

	public PoissonDiscSampling(float radius, int seed, Vector2 sampleRegionSize)
    {
        _radius = radius;
		_seed = seed;
        _sampleRegionSize = sampleRegionSize;
        _numSamplesBeforeRejection = 30;

		prgn = new System.Random(GlobalSeed.Instance.Seed);
    }

    public List<Vector2> PoissonDiscPoints { get; private set; }

	public void GeneratePoints()
	{
		float cellSize = _radius / Mathf.Sqrt(2);

		int[,] grid = new int[Mathf.CeilToInt(_sampleRegionSize.x / cellSize), Mathf.CeilToInt(_sampleRegionSize.y / cellSize)];
		PoissonDiscPoints = new List<Vector2>();
		List<Vector2> spawnPoints = new List<Vector2>();

		spawnPoints.Add(_sampleRegionSize / 2);
        while (spawnPoints.Count > 0)
		{
			int spawnIndex = prgn.Next(0, spawnPoints.Count);
			Vector2 spawnCentre = spawnPoints[spawnIndex];
			bool candidateAccepted = false;

			for (int i = 0; i < _numSamplesBeforeRejection; i++)
			{
				float angle = (prgn.Next(0, 100) / 100f) * Mathf.PI * 2;
				Vector2 dir = new Vector2(Mathf.Sin(angle), Mathf.Cos(angle));
				Vector2 candidate = Vector2Int.FloorToInt(spawnCentre + (dir * prgn.Next(Mathf.FloorToInt(_radius), Mathf.FloorToInt(2 * _radius))));
				if (IsValid(candidate, cellSize, _radius, grid))
				{
					PoissonDiscPoints.Add(candidate);
					spawnPoints.Add(candidate);
					grid[(int)(candidate.x / cellSize), (int)(candidate.y / cellSize)] = PoissonDiscPoints.Count;
					candidateAccepted = true;
					break;
				}
			}
			if (!candidateAccepted)
			{
				spawnPoints.RemoveAt(spawnIndex);
			}

		}
	}

	public void GeneratePoints(int maxPoints = -1)
	{ 
		float cellSize = _radius / Mathf.Sqrt(2);

		int[,] grid = new int[Mathf.CeilToInt(_sampleRegionSize.x / cellSize), Mathf.CeilToInt(_sampleRegionSize.y / cellSize)];
		PoissonDiscPoints = new List<Vector2>();
		List<Vector2> spawnPoints = new List<Vector2>();

		spawnPoints.Add(_sampleRegionSize / 2);
		while (spawnPoints.Count > 0 || maxPoints == PoissonDiscPoints.Count)
		{
			int spawnIndex = prgn.Next(0, spawnPoints.Count);
			Vector2 spawnCentre = spawnPoints[spawnIndex];
			bool candidateAccepted = false;

			for (int i = 0; i < _numSamplesBeforeRejection; i++)
			{
				float angle = (prgn.Next(0,100) / 100f) * Mathf.PI * 2;
				Vector2 dir = new Vector2(Mathf.Sin(angle), Mathf.Cos(angle));
				Vector2 candidate = Vector2Int.FloorToInt(spawnCentre + (dir * prgn.Next(Mathf.FloorToInt(_radius), Mathf.FloorToInt(2 * _radius))));
				if (IsValid(candidate, cellSize, _radius, grid))
				{
					PoissonDiscPoints.Add(candidate);
					spawnPoints.Add(candidate);
					grid[(int)(candidate.x / cellSize), (int)(candidate.y / cellSize)] = PoissonDiscPoints.Count;
					candidateAccepted = true;
					break;
				}
			}
			if (!candidateAccepted)
			{
				spawnPoints.RemoveAt(spawnIndex);
			}

		}

		if (PoissonDiscPoints.Count == 0)
			GeneratePoints(1);

	}

	private bool IsValid(Vector2 candidate, float cellSize, float radius, int[,] grid)
	{
		if (candidate.x >= 0 && candidate.x < _sampleRegionSize.x && candidate.y >= 0 && candidate.y < _sampleRegionSize.y)
		{
			int cellX = (int)(candidate.x / cellSize);
			int cellY = (int)(candidate.y / cellSize);
			int searchStartX = Mathf.Max(0, cellX - 2);
			int searchEndX = Mathf.Min(cellX + 2, grid.GetLength(0) - 1);
			int searchStartY = Mathf.Max(0, cellY - 2);
			int searchEndY = Mathf.Min(cellY + 2, grid.GetLength(1) - 1);

			for (int x = searchStartX; x <= searchEndX; x++)
			{
				for (int y = searchStartY; y <= searchEndY; y++)
				{
					int pointIndex = grid[x, y] - 1;
					if (pointIndex != -1)
					{
						float sqrDst = (candidate - PoissonDiscPoints[pointIndex]).sqrMagnitude;
						if (sqrDst < radius * radius)
						{
							return false;
						}
					}
				}
			}
			return true;
		}
		return false;
	}
}