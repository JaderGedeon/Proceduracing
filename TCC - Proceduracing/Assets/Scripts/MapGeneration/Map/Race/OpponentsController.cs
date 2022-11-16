using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class OpponentsController : MonoBehaviour
{
    [SerializeField] private List<Opponent> opponents;
    [Range(0,1)]
    [SerializeField] private float humanNormalize;
    [Range(0, 1)]
    [SerializeField] private float maxVariation;

    public static float[] opponentsTimes;

    public List<Opponent> Opponents()
    {
        return opponents;
    }

    public void PassTime(int seed, float time)
    {
        System.Random prgn = new System.Random(seed);

        var humanizedTime = time * (1f + humanNormalize);

        var tempList = new List<float>();
        for (int i = 0; i < opponents.Count; i++)
        {
            tempList.Add(prgn.Next(0, (int)(maxVariation * 100)) / 100f + 1f);
        }

        tempList.Sort();

        for (int i = 0; i < opponents.Count; i++)
        {
            opponents[i].AssignTime(humanizedTime * tempList[i]);
        }

        opponentsTimes = new float[opponents.Count];

        for (int i = 0; i < opponentsTimes.Length; i++)
        {
            opponentsTimes[i] = opponents[i].time;
        }
    }


    [System.Serializable]
    public class Opponent
    {
        public float time = 0;
        public Difficulty difficulty;

        public TextMeshProUGUI position;
        public TextMeshProUGUI order;
        public TextMeshProUGUI clock;

        public void AssignTime(float defaultTime)
        {
            time = defaultTime * (1 + ((int)difficulty / 100f));
            var passingTime = time;

            var minutes = Mathf.FloorToInt(passingTime / 60f);
            passingTime -= minutes * 60f;

            var seconds = Mathf.FloorToInt(passingTime);
            passingTime -= seconds;

            passingTime = Mathf.FloorToInt(passingTime * 100f);

            clock.text = minutes.ToString("00") + ":" + seconds.ToString("00") + ":" + passingTime.ToString("00");
        }

        public void MarkOpponent()
        {
            var invisibleColor = new Color(1, 1, 1, 0.235f);

            position.fontStyle = FontStyles.Strikethrough;
            position.color = invisibleColor;

            order.fontStyle = FontStyles.Strikethrough;
            order.color = invisibleColor;

            clock.fontStyle = FontStyles.Strikethrough;
            clock.color = invisibleColor;
        }
    }

    public enum Difficulty
    { 
        Easy = 45,
        Medium = 35,
        Hard = 25,
        Extreme = 15,
        Impossible = 5,
    }

}
