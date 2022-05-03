using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntiRollBar: MonoBehaviour
{

    [SerializeField] private WheelCollider wheelL;
    [SerializeField] private WheelCollider wheelR;
    [SerializeField] private float antiRoll;

    [SerializeField] private Rigidbody rb;

    void FixedUpdate()
    {
        WheelHit hit;
        float travelL = 1.0f;
        var travelR = 1.0f;

        var groundedL = wheelL.GetGroundHit(out hit);

        if (groundedL) {
            travelL = (-wheelL.transform.InverseTransformPoint(hit.point).y - wheelL.radius) / wheelL.suspensionDistance;
        }

        var groundedR = wheelR.GetGroundHit(out hit);

        if (groundedR) {
            travelR = (-wheelR.transform.InverseTransformPoint(hit.point).y - wheelR.radius) / wheelR.suspensionDistance;
        }

        var antiRollForce = (travelL - travelR) * antiRoll;

        if (groundedL) {
            rb.AddForceAtPosition(wheelL.transform.up * -antiRollForce,
                    wheelL.transform.position);
        }

        if (groundedR) {
            rb.AddForceAtPosition(wheelR.transform.up * antiRollForce,
                           wheelR.transform.position);
        }
    }
}
