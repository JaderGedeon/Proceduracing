using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class carUntap : MonoBehaviour
{
    private RidingInputManager ridingInputManager;

    void Start()
    {
        ridingInputManager = GetComponent<RidingInputManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (/*ridingInputManager.untap*/ Input.GetKeyDown(KeyCode.U))
        {
            Vector3 position = gameObject.transform.position;
            position.y += 1;
            //Desvirar voltado para o ponto que ficou caído
            gameObject.transform.SetPositionAndRotation(position, Quaternion.identity);
        }
    }
}
