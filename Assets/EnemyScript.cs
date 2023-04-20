using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{

    [SerializeField] int maxHealth;
    private int currentHealth;
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if(currentHealth <= 0 )
        {
            Die();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            Destroy(transform.root.gameObject);
        }
        else if(collision.gameObject.CompareTag("Bullet"))
        {
            currentHealth--;
        }
    }


    private void Die()
    {
        Destroy(gameObject);
    }


}
