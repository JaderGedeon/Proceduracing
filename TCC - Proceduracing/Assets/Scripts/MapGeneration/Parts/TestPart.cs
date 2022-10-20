using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPart : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log("Apertei");
            PartGenerator.Instance.GeneratePart();
        }
    }
}
