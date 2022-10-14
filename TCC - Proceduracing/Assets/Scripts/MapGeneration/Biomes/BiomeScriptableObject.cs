using UnityEngine;

[CreateAssetMenu(fileName = "Biome-", menuName = "ScriptableObjects/Biome/Biome", order = 1)]
public class BiomeScriptableObject : ScriptableObject
{
    public BiomeType biomeType;
    public Rarity rarity;
    public float heightMultiplier;
    public float friction;
    [Range(0,1)] public float drag;
    public StructureData[] structures;
    public StructureData[] vegetation;
    public Gradient gradient;
}

public enum Rarity
{
    Common,
    Uncommon,
    Rare,
}

public enum BiomeType
{
    Desert,
    Forest,
    Snow,
}