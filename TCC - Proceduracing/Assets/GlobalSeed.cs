using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalSeed : MonoBehaviour
{
    public static GlobalSeed Instance;

    private void Awake()
    {
        Instance = this;
    }

    public int Seed { get; private set; }

    public RaceType RaceType { get; set; }

    public int TournamentSeed { get; private set; }

    public void SetSeed(int seed)
    {
        if (seed > 0)
            Seed = seed;
        else
            GenerateRandomSeed();
    }

    public void GenerateRandomSeed()
    {
        SetSeed(Random.Range(1, 100000));
    }

    public void SetTournamentSeed(int seed)
    {
        if (seed > 0)
            TournamentSeed = seed;
        else
            GenerateRandomTournamentSeed();
    }

    public void GenerateRandomTournamentSeed()
    {
        SetTournamentSeed(Random.Range(1, 100000));
    }
}

public enum RaceType
{ 
    QUICK_RACE,
    TOURNAMENT,
}
