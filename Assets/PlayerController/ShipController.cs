using UnityEngine;

public class ShipController : MonoBehaviour
{
    public float speed = 10f; // Speed of the ship
    public Vector2 mousePos; // Position of the mouse cursor
    private Rigidbody2D rb; // Rigidbody of the ship

    private void Start()
    {
        
        rb = GetComponent<Rigidbody2D>(); // Get the Rigidbody component
    }

    private void Update()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition); // Get the mouse position in world space
    }

    private void FixedUpdate()
    {
        Vector2 direction = mousePos - rb.position; // Calculate direction towards the mouse cursor
        rb.velocity = direction.normalized * speed; // Move the ship towards the mouse cursor

        // Rotate the ship to face the direction of movement
        float angle = Mathf.Atan2(direction.x, -direction.y) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
}
