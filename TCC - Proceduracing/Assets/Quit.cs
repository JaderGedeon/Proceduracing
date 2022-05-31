using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quit : MonoBehaviour
{
    public void QuitApplication()
    {
        Debug.Log("Saiu do joji");
        Application.Quit();
    }
}
