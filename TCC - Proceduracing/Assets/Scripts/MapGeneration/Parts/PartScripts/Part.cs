using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Part
{
    public float Drag { get; set; } = 0;
    public int Mass { get; set; } = 0;
    public int Torque { get; set; } = 0;
    public int BrakeTorque { get; set; } = 0;
    public float Stiffness { get; set; } = 0;
    public PartType Type { get; set; } = PartType.TIRES;
    public PartRarity Rarity { get; set; } = PartRarity.COMMON;
    public GameObject Prefab { get; set; }

    public override string ToString() => $"Type:{Type}, Rarity{Rarity} | Drag:{Drag}, Mass:{Mass}, Torque:{Torque}, BrakeTorque:{BrakeTorque}, Stiffness:{Stiffness}";
}

public enum PartType
{ 
    TIRES,
    CHASSIS,
    SPOILERS,
}

public enum PartRarity
{ 
    COMMON,
    UNCOMMON,
    RARE,
    EPIC,
    LEGENDARY,
}
