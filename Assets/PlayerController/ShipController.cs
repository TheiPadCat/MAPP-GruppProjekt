using System.Collections;
using UnityEngine;

public class ShipController : MonoBehaviour
{
    public float maxSpeed = 15f;
    public AnimationCurve speedCurve;
    public float speedMultiplier = 1f;
    public float speedWhenMouseOver = 0f;
    public float mouseDetectionRadius = 0.5f;

    public float dashVelocity = 20f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 2f;
    private float lastDashTime;

    public Vector2 mousePos;
    private Rigidbody2D rb;

    public float driftForce = 5f;
    public float maxDriftTime = 2f;
    private bool isDrifting = false;
    private float driftTimer = 0f;



    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        /*if (Input.GetKeyDown(KeyCode.Space) && Time.time > lastDashTime + dashCooldown)
        {
            Dash();
            Debug.Log("Dash");
        }*/
        if (Input.GetKey(KeyCode.Space) && rb.velocity.magnitude > 10f)
        {
            isDrifting = true;
            Debug.Log("Drifting is true");
        }
        else
        {
            isDrifting = false;
            driftTimer = 0f;
        }

    }

    private void FixedUpdate()
    {
        Vector2 direction = mousePos - rb.position;

        float distance = Vector2.Distance(mousePos, rb.position);
        float speed = speedCurve.Evaluate(distance) * speedMultiplier;

        if (distance < mouseDetectionRadius)
        {
            speed = speedWhenMouseOver;
        }

        rb.velocity = direction.normalized * speed * maxSpeed;

        float angle = Mathf.Atan2(direction.x, -direction.y) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        if (isDrifting)
        {
            driftTimer += Time.fixedDeltaTime;

            // Gradually increase drift force up to a maximum value
            float currentDriftForce = Mathf.Lerp(0, driftForce, Mathf.Clamp01(driftTimer / maxDriftTime));

            Vector2 driftDirection = Vector3.Cross(transform.up, direction.normalized).z > 0 ? transform.right : -transform.right;
            rb.AddForce(driftDirection * currentDriftForce, ForceMode2D.Force);
            Debug.Log("Drifting");
        }


    }

    private void Dash()
    {
        Vector2 dashDirection = transform.up * dashVelocity;
        rb.velocity += dashDirection;
        lastDashTime = Time.time;
    }
}