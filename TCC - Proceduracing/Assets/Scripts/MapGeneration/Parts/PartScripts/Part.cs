using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Part
{
    public float Drag { get; set; } = 0;
    public int Mass { get; set; } = 900;
    public int Torque { get; set; } = 0;
    public int BrakeTorque { get; set; } = 0;
    public PartType Type { get; set; } = PartType.TIRES;
    public PartRarity Rarity { get; set; } = PartRarity.COMMON;
    public GameObject Prefab { get; set; }

    public Part()
    {

    }

    public Part(PartType partType)
    {
        Type = partType;
    }

    public override string ToString() => $"Type:{Type}, Rarity{Rarity} | Drag:{Drag}, Mass:{Mass}, Torque:{Torque}, BrakeTorque:{BrakeTorque}";
}

public enum PartType
{ 
    TIRES,
    CHASSIS,
    ENGINE,
}

public enum PartRarity
{ 
    COMMON,
    UNCOMMON,
    RARE,
    EPIC,
    LEGENDARY,
}
