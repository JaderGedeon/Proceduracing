using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.SceneManagement;

public class QuickRace : MonoBehaviour
{
    public static int seed = 0;
    public static bool randomSeed = false;

    [SerializeField] private TMP_InputField inputField;

    public void RandomRace()
    {
        randomSeed = true;
        Debug.Log("Corrida Aleatória");
        SceneManager.LoadScene(1);
    }

    public void SeedRace()
    {
        if (inputField.text != "")
        {
            randomSeed = false;
            seed = int.Parse(inputField.text);
            Debug.Log(seed);
            SceneManager.LoadScene(1);
        }
        else {
            Debug.Log("Digite algo");
        }
    }
}
