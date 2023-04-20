using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombShatterScript : MonoBehaviour
{
    // Start is called before the first frame update


    [SerializeField] ParticleSystem explodeParticles;
    void Start()
    {

      //  ParticleSystem explodeParticles = gameObject.GetComponentInChildren<ParticleSystem>();
       // explodeParticles.Emit(39);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnTriggerEnter2D(Collider2D collision) 
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {

            //Wait ?? så den inte collidar direkt med fienden den spawnades från, och gör damage / sprängs
            //enemy.doHarm
          //  ParticleSystem explodeParticles = gameObject.GetComponentInChildren<ParticleSystem>();
            explodeParticles.Emit(39);
            //Sprängas
            //Disable sprite
            //Förstöra

        }
    }

    public void Explode()
    {
        //Trigger particle effect

    }

}
