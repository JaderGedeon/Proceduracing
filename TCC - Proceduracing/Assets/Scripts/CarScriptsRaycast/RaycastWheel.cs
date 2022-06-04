using UnityEngine;
using System.Collections;

public class RaycastWheel : MonoBehaviour
{
	private Rigidbody rb;

	[Header("Suspension")]
	[SerializeField] private float restLength;
	[SerializeField] private float springTravel;
    [SerializeField] private float springStiffness;
    [SerializeField] private float damperStiffness;

    private float minLength;
	private float maxLength;
    private float lastLength;
    private float springLength;
    private float springForce;
    private float springVelocity;
    private float damperForce;

    private RaycastHit hit;
    private Ray ray;

    private Vector3 suspensionForce;

	[Header("Wheels")]
	[SerializeField] private float wheelRadius;

    private void Start()
    {
        rb = transform.root.GetComponent<Rigidbody>();

        minLength = restLength - springTravel;
        maxLength = restLength + springTravel;
    }

    private void FixedUpdate()
    {

        minLength = restLength - springTravel;
        maxLength = restLength + springTravel;

        ray = new Ray(transform.position, -transform.up);

        if (Physics.Raycast(ray, out hit, maxLength + wheelRadius))
        {
            lastLength = springLength;
            springLength = Mathf.Clamp((hit.distance - wheelRadius), minLength, maxLength);
            springVelocity = (lastLength - springLength) / Time.fixedDeltaTime;
            springForce = springStiffness * (restLength - springLength);

            damperForce = damperStiffness * springVelocity;

            suspensionForce = (springForce + damperForce) * transform.up;

            rb.AddForceAtPosition(suspensionForce, hit.point);
        }
    }

    void OnDrawGizmosSelected()
    {
        var touchGroundPoint = new Vector3(transform.position.x, transform.position.y - (maxLength + wheelRadius), transform.position.z);
        var minSpring = new Vector3(transform.position.x, transform.position.y - (minLength), transform.position.z);

        //Gizmos.color = new Color(0, 1, 1, 1);
        //Gizmos.DrawWireSphere(touchGroundPoint, wheelRadius);

        //Gizmos.color = new Color(1, 0, 0, 1);
        //Gizmos.DrawLine(transform.position, touchGroundPoint);

        if (hit.point == Vector3.zero)
        {
            Gizmos.color = new Color(1, 0, 0, 1);
            Gizmos.DrawLine(transform.position, transform.position - transform.up);
        }
        else 
        {
            //Encontrou o ponto de impacto
            Gizmos.color = new Color(1, 1, 0, 1);
            Gizmos.DrawLine(transform.position, hit.point);
        }

        Gizmos.color = new Color(0, 0, 1, 0.7f);
        Gizmos.DrawSphere(hit.point, 0.05f);

        //Gizmos.color = new Color(0, 1, 0, 1);
        //Gizmos.DrawLine(transform.position, new Vector3(transform.position.x, transform.position.y - (maxLength + wheelRadius), transform.position.z));

        //Gizmos.color = new Color(0, 1, 0, 1);
        //var direction = transform.TransformDirection(-Vector3.up);
        //Gizmos.DrawRay(new Vector3(transform.position.x, transform.position.y - radius, transform.position.z), direction);

        //Gizmos.color = new Color(0, 1, 1, 1);
        //direction = transform.TransformDirection(-Vector3.up) * (this.maxSuspension);
        //Gizmos.DrawRay(new Vector3(transform.position.x, transform.position.y - radius, transform.position.z), direction);
    }
}