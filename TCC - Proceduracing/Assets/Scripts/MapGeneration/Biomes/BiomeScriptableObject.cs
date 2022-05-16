using UnityEngine;

[CreateAssetMenu(fileName = "Biome-", menuName = "ScriptableObjects/Biome/Biome", order = 1)]
public class BiomeScriptableObject : ScriptableObject
{
    public Rarity rarity;
    public float heightMultiplier;
    public float friction;
    public StructureData[] structures;
    public StructureData[] vegetation;
    public ColorBiome[] colors;
}

public enum Rarity
{
    Common,
    Uncommon,
    Rare,
}