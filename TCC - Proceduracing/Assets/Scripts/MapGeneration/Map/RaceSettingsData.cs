using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RaceSettingsData : MonoBehaviour
{
    [Header("Race Settings")]
    [SerializeField] private GameObject checkPointGameObject;
    [SerializeField] private int checkPointsAmount;
    [SerializeField] private float minDistanceBetweenPoints;
    [Range(0f, 1f)]
    [SerializeField] private float border;
    [SerializeField] private int checkPointsCollected = 0;
    [SerializeField] private OpponentsController opponentsController;

    private RaceController raceController;

    public Vector2Int[] CheckPointPosition => raceController.CheckPointPosition;

    public int CheckPointsCollected { get => checkPointsCollected; private set => checkPointsCollected = value; }
    public int CheckPointsAmount { get => checkPointsAmount; private set => checkPointsAmount = value; }

    public void Init(Vertex[,] vertexMap, int seed)
    {
        raceController = new RaceController(checkPointGameObject,
            checkPointsAmount, minDistanceBetweenPoints, border);

        raceController.GenerateRace(vertexMap, seed);
    }

    public void PassOpponentsTime(int seed, float averageHeight, float averageFriction)
    {
        opponentsController.PassTime(seed, CalculateRaceTime(averageHeight, averageFriction));
    }

    private float CalculateRaceTime(float averageHeight, float averageFriction)
    {
        float frictionMultiplier = averageFriction >= 1 ? 1f : 2f - averageFriction;
        Debug.Log(frictionMultiplier);
        float heightMultiplier = Mathf.Abs(averageHeight - 0.5f);
        Debug.Log(averageHeight);
        Debug.Log(heightMultiplier);
        float time = CalculateDistanceBetweenCheckPoints() * (frictionMultiplier + heightMultiplier) / 10;
        return time;
    }

    private float CalculateDistanceBetweenCheckPoints()
    {
        List<Vector2Int> possibleCheckPoints = CheckPointPosition.ToList();
        List<Vector2Int> calculatedCheckPoints = new List<Vector2Int>
        {
            possibleCheckPoints[0]
        };
        possibleCheckPoints.RemoveAt(0);

        float finalDistance = 0;

        for (int i = 0; i < calculatedCheckPoints.Count; i++)
        {
            var distance = Mathf.Infinity;
            var index = 0;

            if (possibleCheckPoints.Count != 0)
            {
                for (int j = 0; j < possibleCheckPoints.Count; j++)
                {
                    var checkPointsDistance = Vector2Int.Distance(calculatedCheckPoints[i], possibleCheckPoints[j]);

                    if (checkPointsDistance < distance)
                    {
                        distance = checkPointsDistance;
                        index = j;
                    }
                }
                calculatedCheckPoints.Add(possibleCheckPoints[index]);
                possibleCheckPoints.RemoveAt(index);
                finalDistance += distance;
            }
        }
        return finalDistance;
    }

    private void OnEnable()
    {
        ResetCheckPointsCount();
        CheckPoint.CheckPointCaught += OnCheckPointCollected;
    }

    private void OnDisable()
    {
        CheckPoint.CheckPointCaught -= OnCheckPointCollected;
    }

    private void ResetCheckPointsCount()
    {
        checkPointsCollected = 0;
    }

    private void OnCheckPointCollected()
    {
        checkPointsCollected += 1;
        //UpdateCheckPointUI();

        if (checkPointsCollected == checkPointsAmount)
        {
            //SceneManager.LoadScene(2);
        }

        //Debug.Log(checkPointsCollected);
    }
}
