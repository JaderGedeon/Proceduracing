using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelController : MonoBehaviour
{

    [Header("Colliders")]
    [SerializeField] private WheelCollider[] wheelColliders = new WheelCollider[4];

    [Header("Wheels")]
    [SerializeField] private Transform[] wheels = new Transform[4];

    [Header("Settings")]
    [SerializeField] private int motorTorque;
    [SerializeField] private float steeringMax;

    // Update is called once per frame
    void FixedUpdate()
    {

        AnimateWheels();

        if (Input.GetKey(KeyCode.W))
        {
            for (int i = 0; i < wheelColliders.Length; i++)
            {
                wheelColliders[i].motorTorque = motorTorque;
            }
        }

        if (Input.GetAxis("Horizontal") != 0)
        {
            for (int i = 0; i < wheelColliders.Length - 2; i++)
            {
                wheelColliders[i].steerAngle = Input.GetAxis("Horizontal") * steeringMax;
            }
        }
        else 
        {
            for (int i = 0; i < wheelColliders.Length - 2; i++)
            {
                wheelColliders[i].steerAngle = 0;
            }
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
