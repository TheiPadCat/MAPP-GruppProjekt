using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flamethrower : MonoBehaviour
{
    [SerializeField] private float range = 5f; //hur långt turret kan attackera enemy båt
    [SerializeField] private float fireRate = 0.5f; // cooldown basic
    [SerializeField] private float fireDuration = 3f; //säger sig självt
    [SerializeField] private float damage = 10f; // säger sig självt
    [SerializeField] private float damageMultiplier = 1f; // multiplier för skada som ökar med tiden fienden är i attack area
    [SerializeField] private float areaOfEffect = 1f; // radien av attack arean för turret
    //[SerializeField] private ParticleSystem fireParticles; // partikelsystem för najs som joel får lösa 
    [SerializeField] private Transform noseOfTheTurret; //transform för den del av turret som skjuter elden

    private Transform target; // nuvarande fiende som är targeted av turret
    private float lastFireTime; // när turret senast var aktiverad
    private bool isFiring; // är flamethrower aktiv eller inte

    [SerializeField] ParticleSystem fireParticles;

    void Update()
    {
        
        if (CanFire())
        {
            AimAtTarget();
            FindTarget();

            StartCoroutine(Fire());
        }
    }

    void FindTarget()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, range);
        Debug.Log("Number of colliders detected: " + colliders.Length);
        float closestDistance = float.MaxValue;
        Transform closestTarget = null;

        foreach (Collider2D collider in colliders)
        {
            if (collider.gameObject.CompareTag("Enemy"))
            {
                float distanceToTarget = Vector2.Distance(transform.position, collider.transform.position);
                if (distanceToTarget < closestDistance)
                {
                    closestDistance = distanceToTarget;
                    closestTarget = collider.transform;
                }
            }
        }

        target = closestTarget;

        if (target != null)
        {
            Debug.Log("Target found: " + target.gameObject.name);
        }
        else
        {
            Debug.Log("No target found");
        }
    }

    void AimAtTarget()
    {
        if (target != null)
        {
            Vector3 direction = target.position - noseOfTheTurret.position;
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            noseOfTheTurret.rotation = Quaternion.RotateTowards(noseOfTheTurret.rotation, targetRotation, Time.deltaTime * 360f);


            //transform.right = Vector2.Lerp(transform.right, direction, Time.deltaTime * 3);
        }
    }

    bool CanFire()
    {
        return !isFiring && Time.time - lastFireTime >= fireRate;
    }

    IEnumerator Fire()
    {
        isFiring = true;
        fireParticles.Play();
        fireParticles.transform.rotation = noseOfTheTurret.rotation;

        float timer = 0f;
        while (timer < fireDuration && target != null)
        {
            Collider[] colliders = new Collider[10];
            int numColliders = Physics.OverlapSphereNonAlloc(transform.position, areaOfEffect, colliders);

            for (int i = 0; i < numColliders; i++)
            {
                if (colliders[i].gameObject.CompareTag("Enemy"))
                {
                    float damageAmount = damage * damageMultiplier * Time.deltaTime;
                    //colliders[i].gameObject.GetComponent<Health>().TakeDamage(damageAmount); får lösa sen
                    Debug.Log("enemy is damaged");
                }
            }

            damageMultiplier += Time.deltaTime;
            timer += Time.deltaTime;
            yield return null;
        }

        fireParticles.Stop();
        isFiring = false;
        lastFireTime = Time.time;
        damageMultiplier = 1f;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, areaOfEffect);

        if(target != null)
        {
            Gizmos.DrawLine(transform.position, target.position);
        }
      
    }
}

