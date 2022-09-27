using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GetSeed : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI seed;

    // Start is called before the first frame update
    void Start()
    {
        seed.text = ("#" + GlobalSeed.Seed);
    }
}
