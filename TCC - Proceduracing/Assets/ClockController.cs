using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ClockController : MonoBehaviour
{
    public TextMeshProUGUI clock;
    public float time = 0f;

    // Start is called before the first frame update
    void Start()
    {
        AssignValueInClock();
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        AssignValueInClock();
    }

    public void AssignValueInClock() {
        var passingTime = time;

        var minutes = Mathf.FloorToInt(passingTime / 60f);
        passingTime -= minutes * 60f;

        var seconds = Mathf.FloorToInt(passingTime);
        passingTime -= seconds;

        passingTime = Mathf.FloorToInt(passingTime * 100f);

        clock.text = minutes.ToString("00") + ":" + seconds.ToString("00") + ":" + passingTime.ToString("00");
    }
}