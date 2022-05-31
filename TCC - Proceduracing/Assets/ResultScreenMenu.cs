using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResultScreenMenu : MonoBehaviour
{
    public void BackToMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void PlayAgain()
    {
        QuickRace.randomSeed = false;
        QuickRace.seed = MapController.seed;
        SceneManager.LoadScene(1);
    }
}
