using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PartGenerator : MonoBehaviour
{
    public static PartGenerator Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        DontDestroyOnLoad(gameObject);
    }

    [Header("Points")]
    [SerializeField] private float rankMultiplier = 0.25f;
    [SerializeField] private float eventMultiplier = 0.5f;
    [SerializeField] private float FloorMultiplier = 0.1f;
    [SerializeField] private float PartRarityMultiplier = 0.2f;
    [SerializeField] private int DefaultPoints = 100;

    [Header("Properties Cost")]
    [SerializeField] private float DragCost = 1f;
    [SerializeField] private float MassCost = 2f;
    [SerializeField] private float TorqueCost = 3f;
    [SerializeField] private float BrakeCost = 4f;
    [SerializeField] private float StiffnessCost = 5f;

    private static System.Random prgn;
    private static int avaliablePoints = 0;

    //Transformar em Singleton rapidão
    public void SetRandom()
    { 
        prgn = new System.Random(GlobalSeed.Instance.Seed);
    }

    public Part GeneratePart(PartType type, int floor, int podiumRank, bool IsEvent)
    {
        //prgn = new System.Random(new System.Random().Next(0,99999));
        var partSlots = GeneratePartPropertiesSlots();

        Part part = new Part
        {
            Rarity = (PartRarity)partSlots,
            Type = type,
        };

        PointsCalculate(floor, podiumRank, IsEvent, part.Rarity);
        Debug.Log($"Points to Spend: {avaliablePoints}");
        #region AssignPropertiesValue

        var slotsNumber = new List<int> { 0, 1, 2, 3, 4 };

        for (int i = 0; i <= partSlots; i++)
        {
            var selectedPropertie = prgn.Next(0, slotsNumber.Count());
            var spendAll = i == partSlots;

            switch (slotsNumber[selectedPropertie])
            {
                case 0:
                    part.Drag = AssignPropertie(DragCost, spendAll);
                    break;
                case 1:
                    part.Mass = (int)AssignPropertie(MassCost, spendAll);
                    break;
                case 2:
                    part.Torque = (int)AssignPropertie(TorqueCost, spendAll);
                    break;
                case 3:
                    part.BrakeTorque = (int)AssignPropertie(BrakeCost, spendAll);
                    break;
                case 4:
                    part.Stiffness = AssignPropertie(StiffnessCost, spendAll);
                    break;
            }

            slotsNumber.Remove(slotsNumber[selectedPropertie]);
        }
        #endregion

        // Falta a Mesh
        Debug.Log(part.ToString());

        return part;
    }

    public void PointsCalculate(int currentFloor, int podiumRank, bool IsEvent, PartRarity partRarity)
    {
        var finalMultiplier = 1f;
        finalMultiplier += FloorMultiplier * currentFloor;
        finalMultiplier += podiumRank == 1 ? rankMultiplier : podiumRank == 3 ? -rankMultiplier : 0;
        finalMultiplier += IsEvent ? eventMultiplier : 0;
        finalMultiplier += (int)partRarity * PartRarityMultiplier;
        // Luck Multiplier?

        avaliablePoints = (int)(DefaultPoints * finalMultiplier);
    }

    public int GeneratePartPropertiesSlots()
    {
        var slots = 0;

        while (slots < 4)
        {
            if (prgn.Next(1, 100) < 50)
                break;

            slots++;
        }

        return slots;
    }

    public float AssignPropertie(float cost, bool spendAll)
    {  
        var pointsToSpend = spendAll ? avaliablePoints : prgn.Next(0, avaliablePoints);
        avaliablePoints -= pointsToSpend;
        return pointsToSpend / cost;
    }
}

