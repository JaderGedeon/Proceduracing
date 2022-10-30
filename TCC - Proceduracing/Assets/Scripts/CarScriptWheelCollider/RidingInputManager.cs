using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RidingInputManager : MonoBehaviour
{
    public float acceleration;
    public int brake;
    public float turning;
    public bool untap;

    Quaternion deviceRotation;

    private void Start()
    {
        Input.gyro.enabled = true;
        deviceRotation = DeviceRotation.Get();
    }

    private void Update()
    {
        deviceRotation = DeviceRotation.Get();

        acceleration = deviceRotation.z;
        turning = deviceRotation.x;
    }

    public void Acceleration(InputAction.CallbackContext ctx)
    {
        acceleration = ctx.ReadValue<float>();
    }
    public void Brake(InputAction.CallbackContext ctx)
    {
        brake = ctx.performed ? 1 : 0;
    }
    public void Turning(InputAction.CallbackContext ctx)
    {
        turning = ctx.ReadValue<float>();
    }

    /*
    public void Untap(InputAction.CallbackContext ctx)
    {
        untap = ctx.started || ctx.canceled;
        Debug.Log(!ctx.canceled);
        Debug.Log(ctx.phase != InputActionPhase.Performed);
    }
    */
}
