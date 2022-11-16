using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AudioManager
{
    private static GameObject musicGameObject;

    public enum Sound
    {
        MenuMusic,
        TournamentMusic,
        PauseMusic,

        Gameplay1Music,
        Gameplay2Music,
        Gameplay3Music,

        PartsMusic,

        ResultsVictoryMusic,
        ResultsLostMusic,

        ClickButton,
    }

    public enum AudioType
    {
        Music,
        SFX,
    }

    public static Tuple<AudioClip, float> GetCurrentMusic()
    {
        var source = musicGameObject.GetComponent<AudioSource>();
        return new Tuple<AudioClip, float>(source.clip, source.time);
    }


    public static void PlaySound(Sound sound)
    {
        var soundClip = GetSoundAudioClip(sound);

        GameObject soundGameObject = new GameObject("SoundObject");
        AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();

        if (soundClip.audioType == AudioType.Music)
        {
            audioSource.loop = true;
            audioSource.clip = soundClip.audioClip;
            audioSource.volume = AudioClips.Instance.MusicVolume;
            audioSource.Play();
            UnityEngine.Object.Destroy(musicGameObject);
            musicGameObject = soundGameObject;
        }
        else
        {
            audioSource.PlayOneShot(soundClip.audioClip, AudioClips.Instance.SFXVolume);
            UnityEngine.Object.Destroy(soundGameObject, soundClip.audioClip.length);
        }
    }

    public static void PlaySound(AudioClip clip, float time)
    {
        GameObject soundGameObject = new GameObject("SoundObject");
        AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();

        audioSource.loop = true;
        audioSource.clip = clip;
        audioSource.volume = AudioClips.Instance.MusicVolume;
        audioSource.time = time;
        audioSource.Play();
        UnityEngine.Object.Destroy(musicGameObject);
        musicGameObject = soundGameObject;

    }

    private static AudioClips.SoundAudioClip GetSoundAudioClip(Sound sound)
    {
        foreach (AudioClips.SoundAudioClip soundAudioClip in AudioClips.Instance.soundAudioClipArray)
            if (sound == soundAudioClip.sound)
                return soundAudioClip;

        Debug.LogError("Sound " + sound + " not found!");
        return null;     
    }
}

