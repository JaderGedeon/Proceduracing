using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Part
{
    public float Drag { get; set; }
    public float Mass { get; set; }
    public float Torque { get; set; }
    public float BrakeTorque { get; set; }
    public float Stiffness { get; set; }
    public PartType Type { get; set; }
    public PartRarity Rarity { get; set; }
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
