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
    [SerializeField] ParticleSystem waveParticles;
    [SerializeField] ParticleSystem flashParticles;
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
            collider.GetComponent<IEnemy>().TakeDamage(damage);
        }
        CinemachineCameraShake.Instance.ShakeCamera(5f, .1f);
        explosionParticles.Play();
        smokeParticles.Play();
        fireParticles.Play();
        waveParticles.Play();
        flashParticles.Play();
        explosionParticles.transform.parent = null;
        smokeParticles.transform.parent = null;
        fireParticles.transform.parent = null;
        waveParticles.transform.parent = null;
        flashParticles.transform.parent = null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
