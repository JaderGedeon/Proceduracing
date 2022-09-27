using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.SceneManagement;

public class QuickRace : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputField;

    public void RandomRace()
    {
        GlobalSeed.GenerateRandomSeed();
        Debug.Log("Corrida Aleatória");
        SceneManager.LoadScene(1);
    }

    public void SeedRace()
    {
        if (inputField.text != "")
        {
            GlobalSeed.SetSeed(int.Parse(inputField.text));
            SceneManager.LoadScene(1);
        }
        else {
            Debug.Log("Digite algo");
        }
    }
}
