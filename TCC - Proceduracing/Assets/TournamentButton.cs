using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class TournamentButton : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private Button seedButton;
    [SerializeField] private TextMeshProUGUI seedText;

    public void RandomTournament()
    {
        AudioManager.PlaySound(AudioManager.Sound.ClickButton);
        GlobalSeed.Instance.GenerateRandomTournamentSeed();
        TournamentData.Instance.Init();
        Debug.Log("Corrida Aleatória");
        SceneManager.LoadScene(3);
    }

    public void SeedTournament()
    {
        if (inputField.text != "")
        {
            AudioManager.PlaySound(AudioManager.Sound.ClickButton);
            GlobalSeed.Instance.SetTournamentSeed(int.Parse(inputField.text));
            TournamentData.Instance.Init();
            SceneManager.LoadScene(3);
        }
        else
        {
            Debug.Log(inputField.text);
            Debug.Log("Digite algo");
        }
    }

    public void ChangeButtonAlpha()
    {
        if (inputField.text != "")
        {
            seedButton.interactable = true;
            seedText.alpha = 1;
        }
        else
        {
            seedButton.interactable = false;
            seedText.alpha = 0.2f;
        }
    }
}