using System.Collections;
using System;
using System.Diagnostics;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class RaceController : MonoBehaviour
{
    public static Vector2Int[] GenerateRace(Vertex[,] vertexMap, int seed, GameObject checkPointGameObject, int checkPointsQnt, float minimumDistanceBetweenPoints, float border)
    {
        GameObject[] oldCheckPoints = GameObject.FindGameObjectsWithTag("CheckPoint");
        foreach (var checkPoint in oldCheckPoints)
        {
            DestroyImmediate(checkPoint);
        }

        DestroyImmediate(GameObject.Find("checkPointsGroup"));


        Vector2Int mapSize = new Vector2Int(vertexMap.GetLength(0), vertexMap.GetLength(1));
        Vector2Int[] checkPointsPosition = new Vector2Int[checkPointsQnt];

        List<Vector2Int> posList = new List<Vector2Int>(mapSize.x * mapSize.y);
        for (int y = (int)(vertexMap.GetLength(0) / 2 * border); y < (int)(vertexMap.GetLength(0) / 2 * (2 - border)); y++)
        {
            for (int x = (int)(vertexMap.GetLength(1) / 2 * border); x < (int)(vertexMap.GetLength(1) / 2 * (2 - border)); x++)
            {
                posList.Add(new Vector2Int(x,y));
            }
        }

        System.Random prgn = new System.Random(seed);

        GameObject checkPointsGroup = new GameObject("checkPointsGroup");

        for (int i = 0; i < checkPointsQnt; i++)
        {
            checkPointsPosition[i] = posList[prgn.Next(0, posList.Count)];
            var checkPoint = Instantiate(
                checkPointGameObject,
                new Vector3(checkPointsPosition[i].x, 0, checkPointsPosition[i].y),
                Quaternion.identity, checkPointsGroup.transform);

            var removedPos = new List<Vector2Int>(mapSize.x * mapSize.y);

            for (int j = 0; j < posList.Count; j++)
            {
                var distance = Vector2Int.Distance(posList[j], checkPointsPosition[i]);

                if (distance <= minimumDistanceBetweenPoints)
                    removedPos.Add(posList[j]);
            }

            posList = posList.Except(removedPos).ToList();
        }

        return checkPointsPosition;
    }
}
