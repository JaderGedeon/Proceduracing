using UnityEngine;

[CreateAssetMenu(fileName = "Structure-", menuName = "ScriptableObjects/Biome/Structure", order = 2)]
public class StructureData : ScriptableObject
{
    public GameObject structure;
    public Vector3 maxRotation;
    [Range(0,1)]
    public float density;

    public StructureData(GameObject structure, Vector3 maxRotation, float density)
    {
        this.structure = structure;
        this.maxRotation = maxRotation;
        this.density = density;
    }
}
