using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.SceneManagement;


public class TournamentButton : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputField;

    public void RandomTournament()
    {
        GlobalSeed.Instance.GenerateRandomTournamentSeed();
        TournamentData.Instance.Init();
        Debug.Log("Corrida Aleatória");
        SceneManager.LoadScene(3);
    }

    public void SeedTournament()
    {
        if (inputField.text != "")
        {
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
}