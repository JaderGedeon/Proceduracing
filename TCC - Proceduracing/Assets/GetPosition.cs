using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GetPosition : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI position;
    [SerializeField] private List<GameObject> positionList;

    // Start is called before the first frame update
    void Start()
    {
        position.text = ClockController.position + "�";

        foreach (Transform child in positionList[ClockController.position - 1].transform)
        {
            child.GetComponent<TextMeshProUGUI>().color = new Color(1, 1, 0);
            if (child.name == "clock")
            {
                child.GetComponent<TextMeshProUGUI>().text = ConvertTime(ClockController.time);
            }
        }

        positionList.RemoveAt(ClockController.position - 1);

        var opponents = OpponentsController.opponentsTimes;

        for (int i = 0; i < positionList.Count; i++)
        {
            GameObject position = positionList[i];
            foreach  (Transform child in position.transform)
            {
                if (child.name == "clock")
                {
                    child.GetComponent<TextMeshProUGUI>().text = ConvertTime(opponents[i]);
                }
            }
        }
    }

    public string ConvertTime(float passingTime)
    {
        var minutes = Mathf.FloorToInt(passingTime / 60f);
        passingTime -= minutes * 60f;

        var seconds = Mathf.FloorToInt(passingTime);
        passingTime -= seconds;

        passingTime = Mathf.FloorToInt(passingTime * 100f);

        return minutes.ToString("00") + ":" + seconds.ToString("00") + ":" + passingTime.ToString("00");
    }
}
