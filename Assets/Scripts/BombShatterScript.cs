using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombShatterScript : MonoBehaviour
{
    // Start is called before the first frame update


    [SerializeField] ParticleSystem explodeParticles;
    public int damage;
    public bool canHurtEnemy = false;
    void Start()
    {

      //  ParticleSystem explodeParticles = gameObject.GetComponentInChildren<ParticleSystem>();
       // explodeParticles.Emit(39);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Bomben spr�ngs efter n�gra sekunder

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            if (canHurtEnemy)
            {
                other.gameObject.GetComponent<IEnemy>().TakeDamage(damage);
                print(gameObject + "tried to do damage");
            }
    

        }
    }

    public void Explode()
    {
        canHurtEnemy = true;
        explodeParticles.Emit(15);
        explodeParticles.transform.parent = null;
    }


}
