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

            //Wait ?? s� den inte collidar direkt med fienden den spawnades fr�n, och g�r damage / spr�ngs
            //enemy.doHarm
          //  ParticleSystem explodeParticles = gameObject.GetComponentInChildren<ParticleSystem>();
            explodeParticles.Emit(39);
            //Spr�ngas
            //Disable sprite
            //F�rst�ra

        }
    }

    public void Explode()
    {
        //Trigger particle effect

    }

}
