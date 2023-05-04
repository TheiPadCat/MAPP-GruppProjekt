using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletScript : MonoBehaviour
{
    [SerializeField] float bulletVelocity;
    private Transform target;
    public Vector3 direction;
    public int dmg;


    

    private LayerMask test;
    // Start is called before the first frame update
    void Start()
    {
       

        GetComponent<Rigidbody2D>().velocity = transform.right * bulletVelocity;

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            collision.GetComponent<IEnemy>().TakeDamage(dmg);
            Destroy(gameObject);
        }
        if(collision.gameObject.CompareTag("Border"))
        {
            Destroy(gameObject);
        }
    }

}
