using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SquareCard : MonoBehaviour
{
    [SerializeField] private Image backgroundImage;
    [SerializeField] private Image partIcon;
    [SerializeField] private TextMeshProUGUI partType;

    private PartType type;
    private PartRarity rarity;

    public void Init(PartType type, PartRarity rarity)
    {
        this.type = type;
        this.rarity = rarity;
        AssignCardValues(type, rarity);

        void AssignCardValues(PartType type, PartRarity rarity)
        {
            backgroundImage.color = CardUtils.Instance.RarityToColour(rarity);
            partIcon.sprite = CardUtils.Instance.PartIcon(type);
            partType.text = CardUtils.Instance.PartName(type);
        }
    }
    /*
    public void OnMouseDown()
    {
        TODO
    }
    */

}
