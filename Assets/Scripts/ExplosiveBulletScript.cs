using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ExplosiveBulletScript: MonoBehaviour
{
    [SerializeField] float bulletVelocity;
    private int amountOfShatters;
    private float shatterMoveForce;

    private bool hasInstantiated;

    private Transform target;
    public Vector3 direction;
    public float dmg;

   [SerializeField] GameObject explosivePrefab;

    [SerializeField] LayerMask bulletLayer;
    [SerializeField] LayerMask playerLayer;

    private LayerMask test;


    void Start()
    {
        //Detta borde flyttas till en manager
        Physics2D.IgnoreLayerCollision(6, 8);


        GetComponent<Rigidbody2D>().velocity = direction * bulletVelocity;

    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Vector3 spawnPosition = collision.transform.position;

            if (!hasInstantiated) //Kontrollera att vi inte spawnar för många instanser
            {
                GameObject bomb = Instantiate(explosivePrefab, spawnPosition, Quaternion.identity);

                hasInstantiated = true;

                //Ta bort Bullet objektets barn
                DisableChildren();

                Rigidbody2D bombRb = bomb.GetComponent<Rigidbody2D>();

                bombRb.AddForce(direction * shatterMoveForce);

            }

            //StartCoroutine(SpawnShatterAndDelete());
        }
    }


    private void DisableChildren()
    {
        foreach (Transform child in transform)
            child.gameObject.SetActive(false);
    }
   

}
