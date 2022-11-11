using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AudioManager
{
    private static GameObject musicGameObject;

    public enum Sound
    {
        MenuMusic,
        RunMusic,
        ClickButton,
    }

    public enum AudioType
    {
        Music,
        SFX,
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
            Object.Destroy(musicGameObject);
            musicGameObject = soundGameObject;
        }
        else
        {
            audioSource.PlayOneShot(soundClip.audioClip, AudioClips.Instance.SFXVolume);
            Object.Destroy(soundGameObject, soundClip.audioClip.length);
        }
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

