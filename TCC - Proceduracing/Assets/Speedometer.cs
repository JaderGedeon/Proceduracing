using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Speedometer : MonoBehaviour
{
    float kph = 0;
    private Rigidbody rb;
    [SerializeField] private TextMeshProUGUI velocityLabel;
    [SerializeField] private Camera mainCamera;

    [SerializeField] private float speedLerpDuration = 20f;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void CalculateSpeedOnKpH() {
        kph = Mathf.Lerp(rb.velocity.magnitude * 3.6f, kph, Time.deltaTime * 0.5f);
        velocityLabel.text = $"{kph:0}";
        mainCamera.fieldOfView = Mathf.Lerp(mainCamera.fieldOfView, Mathf.Clamp(kph, 0, 90) / 90 * 45 + 45, speedLerpDuration * Time.deltaTime);
    }

    private void FixedUpdate()
    {
        CalculateSpeedOnKpH();
    }
}
