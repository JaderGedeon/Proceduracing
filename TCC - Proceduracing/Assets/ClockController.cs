using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ClockController : MonoBehaviour
{
    public static int position = 0;

    public TextMeshProUGUI clock;
    public OpponentsController opponentsController;
    public static float time = 0f;

    // Start is called before the first frame update
    void Start()
    {
        time = 0f;
        position = 1;
        AssignValueInClock();
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        AssignValueInClock();

        var opponents = opponentsController.Opponents();

        if (opponents.Count >= position)
        {
            if (opponents[position - 1].time < time)
            {
                opponents[position - 1].MarkOpponent();
                position++;
            }
        }
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