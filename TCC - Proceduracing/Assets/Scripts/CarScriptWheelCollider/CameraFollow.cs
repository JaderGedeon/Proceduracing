using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float lerpTime;
    [SerializeField] private GameObject vehicle;
    [SerializeField] private GameObject focusPoint;

    private void FixedUpdate()
    {
        gameObject.transform.position = Vector3.Lerp(transform.position, focusPoint.transform.position, lerpTime * Time.deltaTime);
        gameObject.transform.LookAt(vehicle.transform.position);
    }
}
