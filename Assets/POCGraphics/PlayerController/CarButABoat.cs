using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarButABoat : MonoBehaviour
{
    public float acceleration;
    public float steering;
    public bool usingMouseInput = true;

    public float dashForce = 5f;

    [SerializeField] float maxVelocity;
    

    private Rigidbody2D rb;

    public bool joystickMode;
    public VirtualJoystick virtualJoystick;
    private Vector2 lastInputDirection = Vector2.up;

    [SerializeField] int driftThreshhold;
    [SerializeField] ParticleSystem driftParticlesLeft;
    [SerializeField] ParticleSystem driftParticlesRight;
    [SerializeField] ParticleSystem rippleParticles;
    [SerializeField] ParticleSystem frontSplashParticles;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    void FixedUpdate()
    {
        Vector2 direction;

        if (usingMouseInput && !joystickMode)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            direction = (mousePos - rb.position).normalized;
            Vector2 speed = direction * acceleration * Vector2.Distance(mousePos, rb.position);
            rb.AddForce(speed);
        }
        else if (joystickMode)
        {

            direction = virtualJoystick.inputVector.normalized;
            Vector2 speed = direction * acceleration;
            rb.AddForce(speed);
            

        }
        else
        {
            if (Input.touchCount > 0)
            {
                Vector2 touchPos = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
                direction = (touchPos - rb.position).normalized;
                Vector2 speed = direction * acceleration * Vector2.Distance(touchPos, rb.position);
                rb.AddForce(speed);
               
            }
            else
            {
                direction = rb.velocity.normalized;
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.velocity += rb.velocity.normalized * dashForce;
        }

        if (virtualJoystick.isDragged)
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;
            Quaternion targetRotation = Quaternion.Euler(0, 0, angle);
            rb.MoveRotation(Quaternion.RotateTowards(transform.rotation, targetRotation, steering));
        }
        else
        {
            direction = rb.velocity.normalized;
        }


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
        

        if(relativeForce.magnitude > driftThreshhold)
        {
          if(relativeForce.x < 0)
            {
                driftParticlesRight.Emit(1);
            }
          else
            {
                driftParticlesLeft.Emit(1);
               
            }
         
          
        }
        

        rb.AddForce(rb.GetRelativeVector(relativeForce));

        if(rb.velocity.magnitude > maxVelocity)
        {
            rb.velocity = rb.velocity.normalized * maxVelocity;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Enemy"))
        {
            rb.velocity = rb.velocity.normalized * maxVelocity;
        }
    }
    private void Update()
    {
        float percent = rb.velocity.magnitude / maxVelocity;
        if(rb.velocity.magnitude > 5)
        {
            rippleParticles.Emit(1);
            frontSplashParticles.Emit(5);
            rippleParticles.startSpeed = 10 * percent;
            frontSplashParticles.startSpeed = 10 * percent;
            frontSplashParticles.startLifetime = (float)0.5 * percent;
        }
      

      
      
    }
}
