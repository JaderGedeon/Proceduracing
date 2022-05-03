using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MassCenter : MonoBehaviour
{

    [SerializeField] private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb.centerOfMass += Vector3.forward;
    }
}
