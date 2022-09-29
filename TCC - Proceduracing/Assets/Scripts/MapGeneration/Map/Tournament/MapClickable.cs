using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MapClickable : MonoBehaviour
{
    private int difficultyFloor;
    private int seed;
    private MapIconType iconType;

    public MapClickable(int difficultyFloor, int seed, MapIconType iconType)
    {
        this.difficultyFloor = difficultyFloor;
        this.seed = seed;
        this.iconType = iconType;
    }

    private void OnMouseDown()
    {
        // Start the race;
    }

}

public enum MapIconType
{ 
    RACE,
    GEAR,
    BOSS
}
