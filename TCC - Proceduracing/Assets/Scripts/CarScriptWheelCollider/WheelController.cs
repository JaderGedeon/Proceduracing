using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelController : MonoBehaviour
{
    private RidingInputManager ridingInputManager;

    [Header("Colliders")]
    [SerializeField] private WheelCollider[] wheelColliders = new WheelCollider[4];

    [Header("Wheels")]
    [SerializeField] private Transform[] wheels = new Transform[4];

    [Header("Settings")]
    [SerializeField] private int motorTorque;
    [SerializeField] private int brakeTorque;
    [SerializeField] private float steeringMax;
    [SerializeField] private float steeringSpeed;

    private void Start()
    {
        ridingInputManager = GetComponent<RidingInputManager>();
    }

    void FixedUpdate()
    {
        AnimateWheels();
        MoveVehicle();
    }

    private void MoveVehicle() 
    {
        for (int i = 0; i < wheelColliders.Length; i++)
        {
            wheelColliders[i].motorTorque = ridingInputManager.acceleration * motorTorque;
            wheelColliders[i].brakeTorque = ridingInputManager.brake * brakeTorque;
        }

        for (int i = 0; i < wheelColliders.Length - 2; i++)
        {
            wheelColliders[i].steerAngle = Mathf.Lerp(wheelColliders[i].steerAngle,
                ridingInputManager.turning * steeringMax,
                steeringSpeed * Time.deltaTime);
        }
    }

    private void AnimateWheels()
    {
        Vector3 wheelPosition = Vector3.zero;
        Quaternion wheelRotation = Quaternion.identity;

        for (int i = 0; i < wheels.Length; i++)
        {
            wheelColliders[i].GetWorldPose(out wheelPosition, out wheelRotation);
            wheels[i].transform.position = wheelPosition;
            wheels[i].transform.rotation = wheelRotation;
        }
    }
}
