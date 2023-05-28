
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinding : MonoBehaviour
{
    [SerializeField] private Transform island;
    [SerializeField] private int maxDistanceToIsland;
    [SerializeField] private float speed = 5.0f;

    private Vector3 direction;
    private Collider2D rockCollider;
    private bool canMove = true;
    private Rigidbody2D rigidbod;
    private float colliderSize = 10f;
    private float lerpSize = 10;
    private LayerMask rockLayerMask;

    private void Start()
    {
        rockLayerMask = LayerMask.GetMask("Obstacle");
       // speed = 5f;
        rigidbod = GetComponent<Rigidbody2D>();
        island = Island.Instance.transform;
    }

    private void FixedUpdate()
    {
        if (Vector2.Distance(island.position, transform.position) <= maxDistanceToIsland)
        {
            canMove = false;
        }
        switch (canMove)
        {
            case true:

                UpdateDirection();
                AvoidObstacles();
                MoveTowardsMiddle();
                break;
            case false:

                MakeStatic();

                break;
        }

    }
    private void MoveInDirection(Vector2 lfDirection)
    {
        direction = Vector2.Lerp(direction, lfDirection, lerpSize).normalized;
    }

    private void UpdateDirection()
    {
        direction = (island.position - transform.position).normalized;
    }
    private void MakeStatic()
    {
        rigidbod.bodyType = RigidbodyType2D.Static;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, colliderSize);
    }



    private void MoveTowardsMiddle()
    {
        Quaternion toRotation = Quaternion.LookRotation(Vector3.forward, direction);
        float angle = Quaternion.Angle(transform.rotation, toRotation);
        float t = Mathf.Clamp01(angle / 180f);
        transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, 0.005f);
        rigidbod.velocity = direction * speed;
    }

    private void AvoidObstacles()
    {
        rockCollider = Physics2D.OverlapCircle(transform.position + direction, colliderSize, rockLayerMask);

        if (rockCollider != null)
        {
            Vector3 leftDirection = new Vector2(-direction.y, direction.x); // Swap x and y components for left direction
            Vector3 rightDirection = new Vector2(direction.y, -direction.x); // Swap x and y components for right direction

            bool canMoveLeft = Physics2D.OverlapCircle(transform.position + leftDirection * colliderSize, colliderSize, LayerMask.GetMask("Obstacle")) == null;
            bool canMoveRight = Physics2D.OverlapCircle(transform.position + rightDirection * colliderSize, colliderSize, LayerMask.GetMask("Obstacle")) == null;

            if (canMoveLeft)
            {
                MoveInDirection(leftDirection);
            }
            else if (canMoveRight)
            {
                MoveInDirection(rightDirection);
            }
        }

        direction.Normalize();
    }

  

}


