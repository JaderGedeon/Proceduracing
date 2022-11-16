using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class carUntap : MonoBehaviour
{
    private Rigidbody rigidbody;

    private float reloadTime = 1f;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && reloadTime < 0f)
        {
            Vector3 position = gameObject.transform.position;
            position.y += 1;
            //Desvirar voltado para o ponto que ficou caído
            gameObject.transform.SetPositionAndRotation(position, gameObject.transform.rotation);
            rigidbody.velocity = Vector3.zero;
            rigidbody.angularVelocity = Vector3.zero;
            reloadTime = 1f;
        }

        if (reloadTime > 0f)
            reloadTime -= Time.deltaTime;
    }
}
