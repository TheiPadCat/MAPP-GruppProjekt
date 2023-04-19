using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarButABoat : MonoBehaviour
{
    public float acceleration;
    public float steering;

    public float dashForce = 5f;

    [SerializeField] float maxVelocity; 

    private Rigidbody2D rb;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()

 
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mousePos - rb.position).normalized;

        Vector2 speed = direction * acceleration * Vector2.Distance(mousePos, rb.position);
        rb.AddForce(speed);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.velocity += rb.velocity.normalized * dashForce;
        }

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;
        Quaternion targetRotation = Quaternion.Euler(0, 0, angle);
        rb.MoveRotation(Quaternion.RotateTowards(transform.rotation, targetRotation, steering));

        Vector2 forward = new Vector2(0.0f, 0.5f);
        float steeringRightAngle;
        if (rb.angularVelocity > 0)
        {
            steeringRightAngle = -90;
        }
        else
        {
            steeringRightAngle = 90;
        }

        Vector2 rightAngleFromForward = Quaternion.AngleAxis(steeringRightAngle, Vector3.forward) * forward;
        Debug.DrawLine((Vector3)rb.position, (Vector3)rb.GetRelativePoint(rightAngleFromForward), Color.green);

        float driftForce = Vector2.Dot(rb.velocity, rb.GetRelativeVector(rightAngleFromForward.normalized));
        Vector2 relativeForce = (rightAngleFromForward.normalized * -1.0f) * (driftForce * 10.0f);
        Debug.DrawLine((Vector3)rb.position, (Vector3)rb.GetRelativePoint(relativeForce), Color.red);


        relativeForce = Vector2.ClampMagnitude(relativeForce, maxVelocity);

        rb.AddForce(rb.GetRelativeVector(relativeForce));





     

    }
}