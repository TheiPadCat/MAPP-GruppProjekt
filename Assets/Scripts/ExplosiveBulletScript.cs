using System;
using System.Collections;
using System.Collections.Generic;
//using UnityEditor.VersionControl;
using UnityEngine;
using static UnityEngine.UIElements.UxmlAttributeDescription;
using UnityEngine.SocialPlatforms;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine.Assertions;

public class ExplosiveBulletScript : MonoBehaviour
{
    [SerializeField] float bulletVelocity;
    [SerializeField] private float shatterForce = 20f;
    [SerializeField] private int shatterTime = 1;
    [SerializeField] private int dmg = 0;
    [SerializeField] private GameObject explosivePrefab;
    [SerializeField] private LayerMask bulletLayer;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private bool hasInstantiated = false;
   
    public Vector3 direction;

    private void Start()
    {
        Physics2D.IgnoreLayerCollision(6, 8);
        GetComponent<Rigidbody2D>().velocity = transform.right * bulletVelocity;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!hasInstantiated && collision.gameObject.CompareTag("Enemy"))
        {
            collision.GetComponent<IEnemy>()?.TakeDamage(dmg);
            Vector3 spawnPosition = collision.transform.position;

            //Instantiate 4 bombs
            GameObject[] bombs = new GameObject[4];

            for (int i = 0; i < bombs.Length; i++)
            {
                bombs[i] = Instantiate(explosivePrefab, spawnPosition, Quaternion.identity);
            }

            // Make the main bomb stop moving & hide it 
            BombStopAndHide();

            //Store the forces to be applied to the bombs
            Vector2[] forces = new Vector2[] { Vector2.right, Vector2.up, Vector2.left, Vector2.down }; // Same as the one i used before with y and x values of 1 and 0 etc, but this is more readable.

            //Shoot the smaller bombs in different directiuons
            for (int i = 0; i < bombs.Length; i++)
            {
                Rigidbody2D bombRb = bombs[i].GetComponent<Rigidbody2D>();
                Vector2 force = forces[i] * shatterForce;
                bombRb.AddForce(force, ForceMode2D.Impulse);
            }

            // Explode bombs 
            for (int i = 0; i < bombs.Length; i++)
            {
                StartCoroutine(ExplodeAfterTime(bombs[i]));
            }

            hasInstantiated = true;
        }
    }
    private IEnumerator ExplodeAfterTime(GameObject bombToExplode)
    {
        //Wait before shatter
        yield return new WaitForSeconds(shatterTime);

        //Explode the bomb
        bombToExplode.GetComponent<BombShatterScript>()?.Explode(); //"Unity objects should not use null propagation" ?

        //Wait so it can do damage before getting destroyed

        //KOMMENTERADE UT DEN HÄR FÖR ATT DEN SKA FÖRSVINNA SAMTIDIGT SOM EFFECTER SPELAR /JEOL
        //yield return new WaitForSeconds(0.3f);

        Destroy(bombToExplode);
        Destroy(gameObject);
    }

   private void BombStopAndHide()
    {
        GetComponent<SpriteRenderer>().enabled = false;
        if (TryGetComponent<Rigidbody2D>(out var rigidbody2D)) //UNT0026 "GetComponent allocates even if no component is found" så körde "potential fixes".
        {
            rigidbody2D.velocity = Vector2.zero;
        }
        //Hide the main bomb (including disabling all of its children objects)
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
    }
}
