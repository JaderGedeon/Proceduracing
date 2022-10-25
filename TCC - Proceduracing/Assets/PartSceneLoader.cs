using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartSceneLoader : MonoBehaviour
{
    [SerializeField] private SquareCard[] squareCards;
    [SerializeField] private CardSceneLoader cardScene;

    private List<Part> parts;

    public static bool isEvent = false;

    void Start()
    {
        CarParts.Instance.floor = TournamentData.Instance.CurrentRoom.Floor;
        CarParts.Instance.podiumRank = ClockController.position;
        Debug.Log(CarParts.Instance.podiumRank);
        CarParts.Instance.isEvent = isEvent;
    
        PartGenerator.Instance.SetRandom();
        parts = new List<Part>
        {
            CarParts.Instance.GeneratePart(PartType.CHASSIS),
            CarParts.Instance.GeneratePart(PartType.TIRES),
            CarParts.Instance.GeneratePart(PartType.ENGINE)
        };

        for (int i = 0; i < squareCards.Length; i++)
            squareCards[i].Init(parts[i]);

    }

    public void SelectPart(int selectedPart)
    {
        cardScene.gameObject.SetActive(true);
        cardScene.Init(parts[selectedPart]);
        gameObject.SetActive(false);
    }
}
