using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PartCard : MonoBehaviour
{
    [SerializeField] private Image partImage;
    [SerializeField] private Image backgroundImage;
    [SerializeField] private TextMeshProUGUI partType;
    [SerializeField] private Image cardBackground;
    [SerializeField] private Image[] stars;
    [SerializeField] private Transform prefabParent;
    [Space(5)]

    [SerializeField] private TextMeshProUGUI torque;
    [SerializeField] private TextMeshProUGUI brake;
    [SerializeField] private TextMeshProUGUI drag;
    [SerializeField] private TextMeshProUGUI mass;


    public void Init(Part part)
    {
        AssignCardValues(part.Type, part.Rarity);

        void AssignCardValues(PartType type, PartRarity rarity)
        {
            partImage.sprite = CardUtils.Instance.PartIcon(type);
            backgroundImage.color = CardUtils.Instance.RarityToBackgroundColour(rarity);
            partType.text = CardUtils.Instance.PartName(type);
            cardBackground.color = CardUtils.Instance.RarityToColour(rarity);

            if (part.Prefab != null)
                Instantiate(part.Prefab, prefabParent);
            else {
                partImage.rectTransform.localPosition = Vector3.zero;
                partImage.rectTransform.localScale = Vector3.one;
            }

            for (int i = (int)rarity; i < 4; i++)
            {
                stars[i].GetComponent<LayoutElement>().ignoreLayout = true;
                stars[i].enabled = false;
            }

            torque.text = $"{part.Torque}";
            brake.text = $"{part.BrakeTorque}";
            drag.text = $"{part.Drag}";
            mass.text = $"{part.Mass}";
        }
    }
}
