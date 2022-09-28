using System.Collections;
using System;
using System.Diagnostics;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class RaceController : MonoBehaviour
{
    private GameObject _checkPointPrefab;
    private int _checkPointAmount;
    private float _minDistBetweenPoints;
    private float _bordesDistance;

    private List<GameObject> _checkPoints;
    private GameObject _checkPointParent;

    public Vector2Int[] CheckPointPosition { get; private set; }

    public RaceController(GameObject checkPointPrefab, int checkPointAmount, float minDistBetweenPoints, float bordesDistance)
    {
        _checkPointPrefab = checkPointPrefab;
        _checkPointAmount = checkPointAmount;
        _minDistBetweenPoints = minDistBetweenPoints;
        _bordesDistance = bordesDistance;

        _checkPoints = new List<GameObject>();
        _checkPointParent = new GameObject("checkPointsGroup");
    }

    public void GenerateRace(Vertex[,] vertexMap, int seed)
    {
        DestroyExistingCheckPoints();

        System.Random prgn = new System.Random(seed);

        Vector2Int mapSize = new Vector2Int(vertexMap.GetLength(0), vertexMap.GetLength(1));
        CheckPointPosition = new Vector2Int[_checkPointAmount + 1];

        List<Vector2Int> posList = new List<Vector2Int>(mapSize.x * mapSize.y);
        for (int y = (int)(vertexMap.GetLength(0) / 2 * _bordesDistance); y < (int)(vertexMap.GetLength(0) / 2 * (2 - _bordesDistance)); y++)
        {
            for (int x = (int)(vertexMap.GetLength(1) / 2 * _bordesDistance); x < (int)(vertexMap.GetLength(1) / 2 * (2 - _bordesDistance)); x++)
            {
                posList.Add(new Vector2Int(x,y));
            }
        }

        for (int i = 0; i < _checkPointAmount + 1; i++)
        {
            CheckPointPosition[i] = posList[prgn.Next(0, posList.Count)];
            if (i != 0)
            {
                Instantiate(
                    _checkPointPrefab,
                    new Vector3(CheckPointPosition[i].x, 0, CheckPointPosition[i].y),
                    Quaternion.identity, _checkPointParent.transform);

                var removedPos = new List<Vector2Int>(mapSize.x * mapSize.y);

                for (int j = 0; j < posList.Count; j++)
                {
                    var distance = Vector2Int.Distance(posList[j], CheckPointPosition[i]);

                    if (distance <= _minDistBetweenPoints)
                        removedPos.Add(posList[j]);
                }

                posList = posList.Except(removedPos).ToList();
            }
        }
    }

    private void DestroyExistingCheckPoints()
    {
        for (int i = 0; i < _checkPoints.Count; i++)
            Destroy(_checkPoints[i]);

        _checkPoints.RemoveAll(c => c == null);
    }
}
