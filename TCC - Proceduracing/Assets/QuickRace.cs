using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class QuickRace : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private Button seedButton;
    [SerializeField] private TextMeshProUGUI seedText;

    public void RandomRace()
    {
        AudioManager.PlaySound(AudioManager.Sound.ClickButton);
        GlobalSeed.Instance.GenerateRandomSeed();
        GlobalSeed.Instance.RaceType = RaceType.QUICK_RACE;
        Debug.Log("Corrida Aleatória");
        SceneManager.LoadScene(1);
    }

    public void SeedRace()
    {
        if (inputField.text != "")
        {
            AudioManager.PlaySound(AudioManager.Sound.ClickButton);
            GlobalSeed.Instance.SetSeed(int.Parse(inputField.text));
            GlobalSeed.Instance.RaceType = RaceType.QUICK_RACE;
            SceneManager.LoadScene(1);
        }
        else {
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
