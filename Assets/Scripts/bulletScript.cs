using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletScript : MonoBehaviour
{
    [SerializeField] float bulletVelocity;
    private Transform target;
    public Vector3 direction;
    public float dmg;


    [SerializeField] LayerMask bulletLayer;
    [SerializeField] LayerMask playerLayer;

    private LayerMask test;
    // Start is called before the first frame update
    void Start()
    {
        //Detta borde flyttas till en manager
        Physics2D.IgnoreLayerCollision(6,8);


        GetComponent<Rigidbody2D>().velocity = direction * bulletVelocity;

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
         
            Destroy(gameObject);
        }
    }

}
