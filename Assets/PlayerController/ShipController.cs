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
    private bool isDrifting = false;

    private float sidewaysForce = 10f;
    private float turnThreshold = 100f;
    private bool isTurning = false;

    private LowPassFilter filter = new LowPassFilter(0.5f);

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
        if (Input.GetKey(KeyCode.Space))
        {
            isDrifting = true;
            Debug.Log("Drifting is true");
        }
        else
        {
            isDrifting = false;
        }

        if (rb.velocity.magnitude > 10f && Mathf.Abs(Input.GetAxisRaw("Horizontal")) > turnThreshold)
        {
            isTurning = true;
            Debug.Log("isTurning = true");
        }
        else
        {
            isTurning = false;
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
            Vector2 driftDirection = Vector3.Cross(transform.up, direction.normalized).z > 0 ? transform.right : -transform.right;
            rb.AddForce(driftDirection * driftForce, ForceMode2D.Force);
            transform.position = filter.Filter(transform.position);
            Debug.Log("Drifting");
        }

        if (isTurning)
        {
            float sideways = Input.GetAxisRaw("Horizontal") * sidewaysForce;
            rb.AddForce(transform.right * sideways, ForceMode2D.Force);
            transform.rotation = Quaternion.Euler(0f, 0f, -90f * Mathf.Sign(sideways));
            Debug.Log("Turning");
        }



    }

    private void Dash()
    {
        Vector2 dashDirection = transform.up * dashVelocity;
        rb.velocity += dashDirection;
        lastDashTime = Time.time;
    }
}

public class LowPassFilter
{
    private float smoothingFactor;
    private Vector3 lastValue;

    public LowPassFilter(float smoothingFactor)
    {
        this.smoothingFactor = smoothingFactor;
    }

    public Vector3 Filter(Vector3 value)
    {
        Vector3 filteredValue = lastValue * smoothingFactor + value * (1 - smoothingFactor);
        lastValue = filteredValue;
        return filteredValue;
    }
}