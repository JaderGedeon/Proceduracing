using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpponentsController : MonoBehaviour
{
    [SerializeField] private Opponent[] opponents;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


    [System.Serializable]
    private class Opponent
    {
        private int time = 0;
        public Difficulty difficulty;

        private void assignTime(float defaultTime)
        {
            time = (int)(defaultTime / ((int)difficulty + 1));
        }
    }

    enum Difficulty
    { 
        Easy,
        Medium,
        Hard,
        Extreme,
        Impossible,
    }

}
