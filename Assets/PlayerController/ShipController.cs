using System.Collections;
using UnityEngine;

public class ShipController : MonoBehaviour
{
    
    public float maxSpeed = 15f; // Speed of the ship
    public AnimationCurve speedCurve;
    public float speedMultiplier = 1f;
    public float speedWhenMouseOver = 0f;
    public float mouseDetectionRadius = 0.5f;

    public float dashForce = 20f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 2f;
    private float lastDashTime;

    public Vector2 mousePos; // Position of the mouse cursor
    private Rigidbody2D rb; // Rigidbody of the ship

    private void Start()
    {
        
        rb = GetComponent<Rigidbody2D>(); // Get the Rigidbody component
    }

    private void Update()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition); // Get the mouse position in world space

        if(Input.GetKeyDown(KeyCode.Space) && Time.time > lastDashTime + dashCooldown)
        {
            StartCoroutine(DashCorutine());
            Debug.Log("Dash");
        }
    }

    private void FixedUpdate()
    {
        Vector2 direction = mousePos - rb.position; // Calculate direction towards the mouse cursor

        float distance = Vector2.Distance(mousePos, rb.position);
        float speed = speedCurve.Evaluate(distance) * speedMultiplier;

        if(distance < mouseDetectionRadius)
        {
            speed = speedWhenMouseOver;
        }
        rb.velocity = direction.normalized * speed; // Move the ship towards the mouse cursor

        // Rotate the ship to face the direction of movement
        float angle = Mathf.Atan2(direction.x, -direction.y) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    private IEnumerator DashCorutine()
    {
        rb.AddForce(transform.up * dashForce, ForceMode2D.Impulse);
        yield return new WaitForSeconds(dashDuration);

        // Set the velocity to the current speed of the ship
        rb.velocity = rb.velocity.normalized * Mathf.Clamp(rb.velocity.magnitude, 0f, maxSpeed);
        lastDashTime = Time.time;
    }
}
