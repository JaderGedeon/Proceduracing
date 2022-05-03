using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RidingInputManager : MonoBehaviour
{
    public float acceleration;
    public int brake;
    public float turning;

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
}
