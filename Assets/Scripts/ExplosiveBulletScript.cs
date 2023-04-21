using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ExplosiveBulletScript: MonoBehaviour
{
    [SerializeField] float bulletVelocity;
    [SerializeField] private float shatterForce = 20f;
    [SerializeField] private int shatterTime = 1;
  
    private bool hasInstantiated;

    private Transform target;
    public Vector3 direction;
    public float dmg;

   [SerializeField] GameObject explosivePrefab;

    [SerializeField] LayerMask bulletLayer;
    [SerializeField] LayerMask playerLayer;


    GameObject bomb1;
    GameObject bomb2;
    GameObject bomb3;
    GameObject bomb4;

    


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
                //Skapa instanser av 4 bomber

                GameObject[] bombList = new GameObject[4];
                for (int i = 0; i< bombList.Length; i++)
                {
                    bombList[i] = Instantiate(explosivePrefab, spawnPosition, Quaternion.identity);

                }

                hasInstantiated = true;

                //Stänga av komponenter i bullet så det inte syns
                DisableChildren();

                //Stanna bullet
                GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0) ;
                
                

                //Hämta alla rigidbody för bomberna
                Rigidbody2D[] bombRbList = new Rigidbody2D[bombList.Length];
                for (int i = 0; i< bombRbList.Length; i++)
                {
                    bombRbList[i] = bombList[i].GetComponent<Rigidbody2D>();

                }

                //Skjuta alla bomber åt varsitt håll
                bombRbList[0].AddForce(new Vector3(shatterForce, 0) * shatterForce, ForceMode2D.Impulse);
                bombRbList[1].AddForce(new Vector3(0, shatterForce) * shatterForce, ForceMode2D.Impulse);
                bombRbList[2].AddForce(new Vector3(-shatterForce, 0) * shatterForce, ForceMode2D.Impulse);
                bombRbList[3].AddForce(new Vector3(0, -shatterForce) * shatterForce, ForceMode2D.Impulse);

                //Spränga bomberna
                for (int i = 0; i < bombList.Length; i++)
                {
                    StartCoroutine(shatterAfterTime(bombList[i]));

                }

    
           
            }

          
        } 
    }

    IEnumerator shatterAfterTime(GameObject bombToExplode)
    {
        yield return new WaitForSeconds(shatterTime);
      
        bombToExplode.GetComponent<BombShatterScript>().Explode();

        yield return new WaitForSeconds(0.3f);
        Destroy(bombToExplode);
        Destroy(gameObject);
        
    }


    private void DisableChildren()
    {
        foreach (Transform child in transform)
            child.gameObject.SetActive(false);
    }
   

    

}
