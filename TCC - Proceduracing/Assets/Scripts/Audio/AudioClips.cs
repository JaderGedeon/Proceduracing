using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioClips : MonoBehaviour
{
    public static AudioClips Instance;

    [Range(0f, 1f)] public float MusicVolume = 0.5f;
    [Range(0f, 1f)] public float SFXVolume = 0.75f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        DontDestroyOnLoad(gameObject);
    }

    public SoundAudioClip[] soundAudioClipArray;

    [System.Serializable]
    public class SoundAudioClip {
        public AudioManager.Sound sound;
        public AudioClip audioClip;
        public AudioManager.AudioType audioType;
    }
}
