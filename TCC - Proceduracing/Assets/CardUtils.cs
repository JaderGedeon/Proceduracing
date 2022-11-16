using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CardUtils : MonoBehaviour
{
    public static CardUtils Instance;

    [SerializeField] private Sprite[] partImages; 

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

    private readonly Dictionary<PartRarity, string> rarityColour = new Dictionary<PartRarity, string>()
    {
         { PartRarity.COMMON,         "#A7A7A7" },
         { PartRarity.UNCOMMON,       "#8EF869" },
         { PartRarity.RARE,           "#6991F8" },
         { PartRarity.EPIC,           "#A869F8" },
         { PartRarity.LEGENDARY,      "#F6DC54" },
     };

    private readonly Dictionary<PartRarity, string> rarityBackgroundColour = new Dictionary<PartRarity, string>()
    {
         { PartRarity.COMMON,         "#8C8C8C" },
         { PartRarity.UNCOMMON,       "#6ECB4D" },
         { PartRarity.RARE,           "#4D70CB" },
         { PartRarity.EPIC,           "#854DCB" },
         { PartRarity.LEGENDARY,      "#CBB74D" },
     };

    private readonly Dictionary<PartType, string> partName = new Dictionary<PartType, string>()
    {
         { PartType.CHASSIS,     "Chassi" },
         { PartType.TIRES,       "Pneus" },
         { PartType.ENGINE,    "Motor" },
     };

    public Color32 RarityToColour(PartRarity rarity)
    {
        if (ColorUtility.TryParseHtmlString(rarityColour[rarity], out Color color)) { }

        return (Color32)color;
    }

    public Color32 RarityToBackgroundColour(PartRarity rarity)
    {
        if (ColorUtility.TryParseHtmlString(rarityBackgroundColour[rarity], out Color color)) { }

        return (Color32)color;
    }

    public string PartName(PartType part) => partName[part];

    public Sprite PartIcon(PartType part)
    {
        return part switch
        {
            PartType.TIRES => partImages[0],
            PartType.CHASSIS => partImages[1],
            PartType.ENGINE => partImages[2],
            _ => partImages[2],
        };
    }
}