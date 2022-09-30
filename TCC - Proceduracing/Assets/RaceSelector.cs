using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RaceSelector : MonoBehaviour
{
    [SerializeField] public GameObject selector;
    [SerializeField] public GameObject QuitButton;
    [SerializeField] public GameObject BackButton;
    [SerializeField] public GameObject quickRace;
    [SerializeField] public GameObject tournament;
    [SerializeField] public TextMeshProUGUI text;

    public void Back()
    {
        selector.SetActive(true);
        QuitButton.SetActive(true);

        BackButton.SetActive(false);
        quickRace.SetActive(false);
        tournament.SetActive(false);

        text.text = "Proceduracing";
    }

    public void QuickRaces()
    {
        selector.SetActive(false);
        QuitButton.SetActive(false);

        BackButton.SetActive(true);
        quickRace.SetActive(true);

        text.text = "Corridas";
    }

    public void Tournaments()
    {
        selector.SetActive(false);
        QuitButton.SetActive(false);

        BackButton.SetActive(true);
        tournament.SetActive(true);

        text.text = "Torneio";
    }

}
