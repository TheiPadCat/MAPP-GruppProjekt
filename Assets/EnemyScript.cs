using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{

    [SerializeField] float maxHealth;
    private float currentHealth;
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            Destroy(transform.root.gameObject);
        }
     
    }


    private void Die()
    {
        Destroy(gameObject);
    }

    public void TakeDamage(float dmg)
    {
        currentHealth -= dmg;
        if (currentHealth <= 0)
        {
            Die();
        }
    }


}
