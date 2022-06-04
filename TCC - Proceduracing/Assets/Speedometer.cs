using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Speedometer : MonoBehaviour
{
    float kph = 0;
    private Rigidbody rb;
    [SerializeField] private TextMeshProUGUI velocityLabel;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void CalculateSpeedOnKpH() {
        kph = Mathf.FloorToInt(Mathf.Lerp(rb.velocity.magnitude * 3.6f, kph, Time.deltaTime * 0.5f));
        velocityLabel.text = kph.ToString();
    }

    private void FixedUpdate()
    {
        CalculateSpeedOnKpH();
    }
}
