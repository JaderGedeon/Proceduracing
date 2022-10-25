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

    public void Init(Part part)
    {
        backgroundImage.color = CardUtils.Instance.RarityToColour(part.Rarity);
        partIcon.sprite = CardUtils.Instance.PartIcon(part.Type);
        partType.text = CardUtils.Instance.PartName(part.Type);
    }
}
