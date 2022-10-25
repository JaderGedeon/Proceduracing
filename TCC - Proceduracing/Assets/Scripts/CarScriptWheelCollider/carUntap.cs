using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class carUntap : MonoBehaviour
{
    private RidingInputManager ridingInputManager;
    private Rigidbody rigidbody;

    void Start()
    {
        ridingInputManager = GetComponent<RidingInputManager>();
        rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (/*ridingInputManager.untap*/ Input.GetKeyDown(KeyCode.R))
        {
            Vector3 position = gameObject.transform.position;
            position.y += 1;
            //Desvirar voltado para o ponto que ficou caído
            gameObject.transform.SetPositionAndRotation(position, gameObject.transform.rotation);
            rigidbody.velocity = Vector3.zero;
            rigidbody.angularVelocity = Vector3.zero;

        }
    }
}
