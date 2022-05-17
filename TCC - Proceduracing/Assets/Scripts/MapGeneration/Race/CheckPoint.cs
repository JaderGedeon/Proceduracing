using System;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    private bool status = true;

    [SerializeField] private Material disabledMaterial;

    public static event Action CheckPointCaught;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && status)
        {
            DisableCheckPoint();
            CheckPointCaught?.Invoke();
        }
    }

    private void DisableCheckPoint()
    {
        gameObject.GetComponent<Renderer>().material = disabledMaterial;
        status = false;
    }
}
