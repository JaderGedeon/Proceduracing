using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSound : MonoBehaviour
{
    [SerializeField] private AudioManager.Sound sound;

    private void OnMouseUpAsButton()
    {
        AudioManager.PlaySound(sound);
    }
}
