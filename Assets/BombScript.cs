using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombScript : MonoBehaviour
{
    [SerializeField] float explosionRadius;
    [SerializeField] LayerMask enemyLayer;
    [SerializeField] int damage;
    [SerializeField] ParticleSystem explosionParticles;
    [SerializeField] ParticleSystem smokeParticles;
    [SerializeField] ParticleSystem fireParticles;
    // Start is called before the first frame update
    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    public void Explode()
    {
        Debug.Log("explode");
        Collider2D[] collider2Ds = Physics2D.OverlapCircleAll(transform.position, explosionRadius, enemyLayer);
        foreach(Collider2D collider in collider2Ds)
        {
         
            Debug.Log("TAKE DAMGAGE");
            collider.GetComponent<EnemyScript>().TakeDamage(damage);
        }
        explosionParticles.Emit(20);
        smokeParticles.Emit(20);
        fireParticles.Emit(20);
        explosionParticles.transform.parent = null;
        smokeParticles.transform.parent = null;
        fireParticles.transform.parent = null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
