using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPlayerCollision : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D collision)
    {
         if (collision.gameObject.CompareTag("Player"))
        {
            if(collision.isTrigger)
            {
                Debug.Log("ENEMY COLLISIO");
                transform.root.GetComponent<EnemyScript>().TakeDamage(collision.gameObject.GetComponent<CarButABoat>().dmg);
               // collision.GetComponent<Rigidbody2D>().velocity = collision.GetComponent<CarButABoat>().
            }
           


        }
            
    }
}
