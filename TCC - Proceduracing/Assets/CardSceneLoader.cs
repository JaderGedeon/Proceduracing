using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CardSceneLoader : MonoBehaviour
{
    [SerializeField] private PartCard newPartCard;
    [SerializeField] private PartCard currentPartCard;
    
    private Part part;

    public void Init(Part part)
    {
        this.part = part;
        newPartCard.Init(part);

        switch (part.Type)
        {
            case PartType.TIRES:
                currentPartCard.Init(CarParts.Instance.Tires);
                return;
            case PartType.CHASSIS:
                currentPartCard.Init(CarParts.Instance.Chassi);
                return;
            case PartType.ENGINE:
                currentPartCard.Init(CarParts.Instance.Engine);
                return;
        }
    }

    public void BackToTournament()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(3);
    }

    public void ApplyPartAndBackToTournament()
    {
        switch (part.Type)
        {
            case PartType.TIRES:
                CarParts.Instance.Tires = part;
                break;
            case PartType.CHASSIS:
                CarParts.Instance.Chassi = part;
                break;
            case PartType.ENGINE:
                CarParts.Instance.Engine = part;
                break;
        }

        BackToTournament();
    }
}
