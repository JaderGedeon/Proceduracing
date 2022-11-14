using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    private bool isPaused = false;

    [SerializeField] private GameObject GUI;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject playAgain;

    private AudioClip clip;
    private float time;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Pause();
        }
    }

    public void Pause()
    {
        isPaused = !isPaused;

        GUI.SetActive(!isPaused);
        pauseMenu.SetActive(isPaused);

        if (GlobalSeed.Instance.RaceType == RaceType.TOURNAMENT)
            playAgain.SetActive(false);

        Time.timeScale = isPaused ? 0 : 1;

        if (isPaused)
        {
            var currentMusic = AudioManager.GetCurrentMusic();
            clip = currentMusic.Item1;
            time = currentMusic.Item2;
            AudioManager.PlaySound(AudioManager.Sound.PauseMusic);
        }
        else {
            AudioManager.PlaySound(clip, time);
        }
    }
}
