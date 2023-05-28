using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class teslaCoil : MonoBehaviour
{
    private Collider2D colliderArea;
    [SerializeField] ParticleSystem electricityParticles;
    private List<Collider2D> enemiesInRange = new List<Collider2D>();
    public float damage;
    public float fireRate;
    private float fireCoolDown;
    // Start is called before the first frame update
    void Start()
    {

        colliderArea = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {


        if (fireCoolDown <= 0f)
        {
            Shoot();


           
        }

        fireCoolDown -= Time.deltaTime;
    }

    private void Shoot()
    {
        

        if(enemiesInRange.Count > 0)
       {

           List<Collider2D> tempList = new List<Collider2D>(enemiesInRange);
            foreach (Collider2D enemy in tempList)
         {
               
                if (enemy.GetComponentInChildren<IEnemy>() != null)
                {
                    GetComponent<AudioSource>().Play();
                    enemy.GetComponentInChildren<IEnemy>().TakeDamage(damage);

                }
           
         }
            
       }
        fireCoolDown = 1f / fireRate;

    }

    private void OnTriggerEnter2D(Collider2D collision  )
    {
        if(collision.gameObject.CompareTag("Enemy") && !collision.isTrigger)
        {
       

            enemiesInRange.Add(collision);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && !collision.isTrigger)
        {
            enemiesInRange.Remove(collision);
        }
    }
}
