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
    private Part _engine = new Part();

    public Part Chassi { get => _chassi; set => _chassi = value; }
    public Part Tires { get => _tires; set => _tires = value; }
    public Part Engine { get => _engine; set => _engine = value; }

    public int floor = 0;
    public int podiumRank = 0;
    public bool isEvent = false;

    public Part PartsSum
    {
        get
        {
            var drag = new float?[] { Chassi.Drag, Tires.Drag, Engine.Drag };
            var mass = new int?[] { Chassi.Mass, Tires.Mass, Engine.Mass };
            var torque = new int?[] { Chassi.Torque, Tires.Torque, Engine.Torque };
            var brakeTorque = new int?[] { Chassi.BrakeTorque, Tires.BrakeTorque, Engine.BrakeTorque };

            return new Part()
            {
                Drag = drag.Sum(i => i.GetValueOrDefault()),
                Mass = mass.Sum(i => i.GetValueOrDefault()),
                Torque = torque.Sum(i => i.GetValueOrDefault()),
                BrakeTorque = brakeTorque.Sum(i => i.GetValueOrDefault()),
            };
        }
    }

    public void ResetParts()
    {
        Chassi = new Part(PartType.CHASSIS);
        Tires = new Part(PartType.TIRES);
        Engine = new Part(PartType.ENGINE);
        floor = 0;
        podiumRank = 0;
        isEvent = false;
    }

    public void GenerateParts()
    {
        PartGenerator.Instance.SetRandom();
        Chassi = PartGenerator.Instance.GeneratePart(PartType.CHASSIS, 1, 1, false);
        Tires = PartGenerator.Instance.GeneratePart(PartType.TIRES, 1, 1, false);
        Engine = PartGenerator.Instance.GeneratePart(PartType.ENGINE, 1, 1, false);
    }

    public Part GeneratePart(PartType type) => PartGenerator.Instance.GeneratePart(type, floor, podiumRank, isEvent);

    public void ApplyPart(Part part)
    {
        switch (part.Type)
        {
            case PartType.TIRES:
                Tires = part;
                break;
            case PartType.CHASSIS:
                Chassi = part;
                break;
            case PartType.ENGINE:
                Engine = part;
                break;
        }
    }

    void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
            ResetParts();
    }
}