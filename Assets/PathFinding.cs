using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using static TreeEditor.TreeEditorHelper;
using static UnityEditor.FilePathAttribute;

public class PathFinding : MonoBehaviour
{
    [SerializeField] Transform island;
    List<Transform> path;
    LayerMask rockLayerMask;
    public float speed = 5.0f;
    private Vector3 direction;
    Collider2D rockCollider;
    [SerializeField] int maxDistanceToIsland;
    private bool canMove = true;
    private Rigidbody2D rigidbod;
    float colliderSize = 5f;
    float lerpSize = 2;
    [SerializeField] private float rotationSpeed = 2f;
    private void Start()
    {
        speed = 5f;
        rigidbod = GetComponent<Rigidbody2D>();
        direction = (island.transform.position - transform.position).normalized;
    }
    void FixedUpdate()
    {

        if (direction != Vector3.zero)
        {

            Quaternion toRotation = Quaternion.LookRotation(Vector3.forward, direction);
            float angle = Quaternion.Angle(transform.rotation, toRotation);
            float t = Mathf.Clamp01(angle / 180f); // interpolate faster when angle is smaller
            transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, 0.2f);
            //t* rotationSpeed *Time.fixedDeltaTime
            //Quaternion toRotation = Quaternion.LookRotation(Vector3.forward, direction);
            //transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime); 
            //transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }
      
        Debug.Log(canMove);

        if (Vector2.Distance(island.transform.position, transform.position) <= maxDistanceToIsland)
        {
            canMove = false;    
        }

        if (canMove)
        {
            direction = (island.transform.position - transform.position).normalized;
        }
        

        if (canMove)
        {
            //MoveTowardsMiddle(); 
        }

        rockCollider = Physics2D.OverlapCircle(transform.position + direction, colliderSize, LayerMask.GetMask("Obstacle"));

        if (rockCollider != null)
        {
            Vector3 leftDirection = new Vector2(-direction.y, direction.x); // Swap x and y components for left direction
            Vector3 rightDirection = new Vector2(direction.y, -direction.x); // Swap x and y components for right direction

            bool canMoveLeft = Physics2D.OverlapCircle(transform.position + leftDirection * colliderSize, colliderSize, LayerMask.GetMask("Obstacle")) == null;
            bool canMoveRight = Physics2D.OverlapCircle(transform.position + rightDirection * colliderSize, colliderSize, LayerMask.GetMask("Obstacle")) == null;

            if (canMoveLeft)
            {
                direction = Vector2.Lerp(direction, leftDirection, lerpSize).normalized;
                // direction = leftDirection;

                Debug.Log(leftDirection);
                Debug.Log("Trying to move left");
            }
            else if (canMoveRight)
            {
                direction = Vector2.Lerp(direction, rightDirection, lerpSize).normalized;
                //direction = rightDirection;
                Debug.Log(rightDirection);
                Debug.Log("Trying to move right");
            }
            else
            {
              //  Destroy(gameObject);
            }

            direction.Normalize();
        }


        if (canMove)
        {
            rigidbod.velocity = direction * speed;
        }
        if (!canMove)
        {
            rigidbod.bodyType = RigidbodyType2D.Static;
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, colliderSize);
    }

 
}
    


