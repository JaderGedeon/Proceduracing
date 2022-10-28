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
    [SerializeField] private Image[] stars;

    public void Init(Part part)
    {
        backgroundImage.color = CardUtils.Instance.RarityToColour(part.Rarity);
        partIcon.sprite = CardUtils.Instance.PartIcon(part.Type);
        partType.text = CardUtils.Instance.PartName(part.Type);

        for (int i = (int)part.Rarity; i < 4; i++)
        {
            stars[i].GetComponent<LayoutElement>().ignoreLayout = true;
            stars[i].enabled = false;
        }
    }
}
