using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelController : MonoBehaviour
{
    private RidingInputManager ridingInputManager;
    private Vertex[,] mapFriction;
    private CarParts carParts;

    [Header("Colliders")]
    [SerializeField] private WheelCollider[] wheelColliders = new WheelCollider[4];

    [Header("Wheels")]
    [SerializeField] private Transform[] wheels = new Transform[4];

    [Header("Settings")]
    [SerializeField] private int motorTorque; // Torque
    [SerializeField] private int brakeTorque; // BrakeTorque
    [SerializeField] private float steeringMax;
    [SerializeField] private float steeringSpeed;
    [SerializeField] private Rigidbody carRigidbody; // Mass & Drag

    [Header("Other")]
    [SerializeField] private Transform structuresParent;

    [Header("Chassi")]
    [SerializeField] private Transform Chassi;
    [SerializeField] private Transform Wheel;

    [Header("FUNCIONA")]
    [SerializeField] private GameObject ChassiPrefab;
    [SerializeField] private GameObject WheelPrefab;

    private float carPartDrag;

    public bool IsCarOnGround = false;

    private Vertex[,] MapFrictionInfo { get => mapFriction; set => mapFriction = value; }

    private void Start()
    {
        ridingInputManager = GetComponent<RidingInputManager>();
    }

    void FixedUpdate()
    {
        UpdateWheelFricton();
        AnimateWheels();
        MoveVehicle();
    }

    public void Init(Vertex[,] vertexMap, Vector2Int startPosition) 
    { 
        gameObject.transform.position = 
            new Vector3(startPosition.x, gameObject.transform.position.y, startPosition.y);

        MapFrictionInfo = vertexMap;

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 5f);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.transform.parent == structuresParent)
                Destroy(hitCollider.gameObject);
        }

        ApplyCarParts();
    }

    public void ApplyCarParts()
    {
        carParts = CarParts.Instance;

        if (carParts.Chassi.Prefab == null)
            carParts.Chassi.Prefab = ChassiPrefab;

        if (carParts.Tires.Prefab == null)
            carParts.Tires.Prefab = WheelPrefab;

        Instantiate(carParts.Chassi.Prefab, Chassi);
        Instantiate(carParts.Tires.Prefab, Wheel);
        motorTorque += carParts.PartsSum.Torque;
        brakeTorque += carParts.PartsSum.BrakeTorque;
        carRigidbody.mass = carParts.PartsSum.Mass;
        carPartDrag = carParts.PartsSum.Drag;
    }

    private void UpdateWheelFricton()
    {
        IsCarOnGround = false;

        for (int i = 0; i < wheelColliders.Length; i++)
        {
            if (wheelColliders[i].isGrounded && MapFrictionInfo != null)
            {
                IsCarOnGround = true;
                wheelColliders[i].GetGroundHit(out WheelHit hit);
                WheelFrictionCurve frictionCurve = wheelColliders[i].forwardFriction;
                frictionCurve.stiffness = mapFriction[(int)hit.point.x, (int)hit.point.z].friction; // Stiffness
                // QQ coisa, divide pela escala  /
                wheelColliders[i].forwardFriction = frictionCurve;
                wheelColliders[i].sidewaysFriction = frictionCurve;
                carRigidbody.drag = MapFrictionInfo[(int)hit.point.x, (int)hit.point.z].drag + (carPartDrag / 1000);
            }
        }
        if (!IsCarOnGround) {
            var carRotation = carRigidbody.transform.rotation;
            carRigidbody.drag = 0;
            carRigidbody.rotation = Quaternion.Lerp(carRotation, Quaternion.Euler(0, carRotation.eulerAngles.y, 0), Time.deltaTime * 4f); //3
        }
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

        if (Wheel.GetChild(0).childCount == 1)
        {
            wheelColliders[0].GetWorldPose(out wheelPosition, out wheelRotation);
            wheels[0].transform.SetPositionAndRotation(wheelPosition, wheelRotation);
            //Wheel.GetChild(0).rotation = Quaternion.Euler(Wheel.GetChild(0).rotation.eulerAngles.x, wheelRotation.y, Wheel.GetChild(0).rotation.eulerAngles.z);
            return;
        }

        for (int i = 0; i < wheels.Length; i++)
        {
            wheelColliders[i].GetWorldPose(out wheelPosition, out wheelRotation);
            wheels[i].transform.position = wheelPosition;
            wheels[i].transform.rotation = wheelRotation;
            Wheel.GetChild(0).GetChild(i).SetPositionAndRotation(wheelPosition, wheelRotation);
        }


        /*
        for (int i = 0; i < wheels.Length; i++)
        {
            wheelColliders[i].GetWorldPose(out wheelPosition, out wheelRotation);
            wheels[i].transform.position = wheelPosition;
            wheels[i].transform.rotation = wheelRotation;
        }
        */
    }
}