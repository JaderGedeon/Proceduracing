using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GlobalSeed
{
    public static int Seed { get; private set; }

    public static int TournamentSeed { get; private set; }

    public static void SetSeed(int seed)
    {
        if (seed > 0)
            Seed = seed;
        else
            GenerateRandomSeed();
    }

    public static void GenerateRandomSeed()
    {
        SetSeed(Random.Range(1, 100000));
    }

    public static void SetTournamentSeed(int seed)
    {
        if (seed > 0)
            TournamentSeed = seed;
        else
            GenerateRandomTournamentSeed();
    }

    public static void GenerateRandomTournamentSeed()
    {
        SetTournamentSeed(Random.Range(1, 100000));
    }
}
