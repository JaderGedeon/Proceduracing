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
    [SerializeField] public GameObject tutorial;
    [SerializeField] public TextMeshProUGUI text;

    private void Start()
    {
        AudioManager.PlaySound(AudioManager.Sound.MenuMusic);
    }

    public void Back()
    {
        selector.SetActive(true);
        QuitButton.SetActive(true);

        BackButton.SetActive(false);
        quickRace.SetActive(false);
        tournament.SetActive(false);
        tutorial.SetActive(false);

        text.text = "Proceduracing";
        PlayButtonSound();
    }

    public void QuickRaces()
    {
        selector.SetActive(false);
        QuitButton.SetActive(false);

        BackButton.SetActive(true);
        quickRace.SetActive(true);

        text.text = "Corrida Rápida";
        PlayButtonSound();
    }

    public void Tournaments()
    {
        selector.SetActive(false);
        QuitButton.SetActive(false);

        BackButton.SetActive(true);
        tournament.SetActive(true);

        text.text = "Torneio";
        PlayButtonSound();
    }

    public void Tutorial()
    {
        selector.SetActive(false);
        QuitButton.SetActive(false);

        BackButton.SetActive(true);
        tutorial.SetActive(true);

        text.text = "Tutorial";
        PlayButtonSound();
    }

    public void PlayButtonSound() => AudioManager.PlaySound(AudioManager.Sound.ClickButton);

}
