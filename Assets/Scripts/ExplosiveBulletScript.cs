using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ExplosiveBulletScript: MonoBehaviour
{
    [SerializeField] float bulletVelocity;
    [SerializeField] private float shatterForce = 20f;
    [SerializeField] private int shatterTime = 2;
    private int amountOfShatters;
  
    private bool hasInstantiated;

    private Transform target;
    public Vector3 direction;
    public float dmg;

   [SerializeField] GameObject explosivePrefab;

    [SerializeField] LayerMask bulletLayer;
    [SerializeField] LayerMask playerLayer;

    private LayerMask test;

    private GameObject bomb;


    void Start()
    {
        //Detta borde flyttas till en manager
        Physics2D.IgnoreLayerCollision(6, 8);

       
        GetComponent<Rigidbody2D>().velocity = transform.right * bulletVelocity;

    }


    private void OnTriggerEnter2D(Collider2D collision) /// TO DO: FIXA METODER / LOOPAR IST FÖR BA DUMPA KOD
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Vector3 spawnPosition = collision.transform.position;

            if (!hasInstantiated) //Kontrollera att vi inte spawnar för många instanser
            {
                //Skapa instanser av alla bomber
               
               GameObject bomb1 = Instantiate(explosivePrefab, spawnPosition, Quaternion.identity);
               GameObject bomb2 = Instantiate(explosivePrefab, spawnPosition, Quaternion.identity);
               GameObject bomb3 = Instantiate(explosivePrefab, spawnPosition, Quaternion.identity);
               GameObject bomb4 = Instantiate(explosivePrefab, spawnPosition, Quaternion.identity);
                hasInstantiated = true;

                //Ta bort Bullet objektets barn
                GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0) ;
                DisableChildren();

                //Hämta alla rigidbody
                Rigidbody2D bomb1Rb = bomb1.GetComponent<Rigidbody2D>();
                Rigidbody2D bomb2Rb = bomb2.GetComponent<Rigidbody2D>();
                Rigidbody2D bomb3Rb = bomb3.GetComponent<Rigidbody2D>();
                Rigidbody2D bomb4Rb = bomb4.GetComponent<Rigidbody2D>();

                //Skjuta alla åt varsitt håll
                bomb1Rb.AddForce(new Vector3(shatterForce, 0) * shatterForce, ForceMode2D.Impulse);
                bomb2Rb.AddForce(new Vector3(0, shatterForce) * shatterForce, ForceMode2D.Impulse);
                bomb3Rb.AddForce(new Vector3(-shatterForce, 0) * shatterForce, ForceMode2D.Impulse);
                bomb4Rb.AddForce(new Vector3(0, -shatterForce) * shatterForce, ForceMode2D.Impulse);

                //Sprängas efter 1 sekund

                StartCoroutine(shatterAfterTime(bomb1));
                StartCoroutine(shatterAfterTime(bomb2));
                StartCoroutine(shatterAfterTime(bomb3));
                StartCoroutine(shatterAfterTime(bomb4));

                //Göra damage till fiender


            }

          
        }
    }

    IEnumerator shatterAfterTime(GameObject bombToExplode)
    {
        yield return new WaitForSeconds(shatterTime);
        //Sprängas
        print(bombToExplode + " sprängs");

        yield return new WaitForSeconds(1);
        
        Destroy(gameObject);
        Destroy(bombToExplode);

        print("Förstör bomb + bullet");
    }


    private void DisableChildren()
    {
        foreach (Transform child in transform)
            child.gameObject.SetActive(false);
    }
   

    

}
