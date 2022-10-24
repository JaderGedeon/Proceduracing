using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class CarParts : MonoBehaviour
{
    public static CarParts Instance;

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

    private Part _chassi = new Part();
    private Part _tires = new Part();
    private Part _spoilers = new Part();

    public Part Chassi { get => _chassi; set => _chassi = value; }
    public Part Tires { get => _tires; set => _tires = value; }
    public Part Spoilers { get => _spoilers; set => _spoilers = value; }

    public Part PartsSum
    {
        get
        {
            var drag = new float?[] { Chassi.Drag, Tires.Drag, Spoilers.Drag };
            var mass = new int?[] { Chassi.Mass, Tires.Mass, Spoilers.Mass };
            var torque = new int?[] { Chassi.Torque, Tires.Torque, Spoilers.Torque };
            var brakeTorque = new int?[] { Chassi.BrakeTorque, Tires.BrakeTorque, Spoilers.BrakeTorque };
            var stiffness = new float?[] { Chassi.Stiffness, Tires.Stiffness, Spoilers.Stiffness };

            return new Part()
            {
                Drag = drag.Sum(i => i.GetValueOrDefault()),
                Mass = mass.Sum(i => i.GetValueOrDefault()),
                Torque = torque.Sum(i => i.GetValueOrDefault()),
                BrakeTorque = brakeTorque.Sum(i => i.GetValueOrDefault()),
                Stiffness = drag.Sum(i => i.GetValueOrDefault()),
            };
        }
    }

    public void ResetParts()
    {
        Chassi = new Part();
        Tires = new Part();
        Spoilers = new Part();
    }

    public void GenerateParts()
    {
        PartGenerator.Instance.SetRandom();
        Chassi = PartGenerator.Instance.GeneratePart(PartType.CHASSIS, 1, 1, false);
        Tires = PartGenerator.Instance.GeneratePart(PartType.TIRES, 1, 1, false);
        Spoilers = PartGenerator.Instance.GeneratePart(PartType.SPOILERS, 1, 1, false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
            GenerateParts();

        if (SceneManager.GetActiveScene().buildIndex == 0)
            ResetParts();
    }
}