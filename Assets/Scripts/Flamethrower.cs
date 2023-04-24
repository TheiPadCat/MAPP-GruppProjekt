using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flamethrower : MonoBehaviour
{
    public float fireRate;
    public float maxLifeTime;
    private float currentLifeTime;
    private bool lifeTimeActive;
    private Transform target;
    
    [SerializeField] Transform baseIsland;
    [SerializeField] float rangeRadius;
    [SerializeField] float maxDamage;
    [SerializeField] float damageRate;
    [SerializeField] ContactFilter2D contactFilter;

    private CircleCollider2D scanArea;
    private List<Collider2D> targetList = new List<Collider2D>();
    

    //[SerializeField] ParticleSystem flameParticles;

    private float fireCoolDown;

    // Start is called before the first frame update
    void Start()
    {
        baseIsland = GameObject.Find("Island").transform;

        scanArea = GetComponent<CircleCollider2D>();    
        scanArea.radius = rangeRadius;

        fireCoolDown = 0;

        currentLifeTime = maxLifeTime;
    }

    // Update is called once per frame
    void Update()
    {
        scanArea.OverlapCollider(contactFilter, targetList);

        if (targetList.Count > 0)
        {
            FindTargets();
        }

        if (fireCoolDown <= 0f && targetList.Count > 0)
        {
            Attack();
            fireCoolDown = 1f / fireRate;
        }

        fireCoolDown -= Time.deltaTime;

        if (lifeTimeActive)
        {
            currentLifeTime -= Time.deltaTime;
        }

        if (currentLifeTime <= 0f && lifeTimeActive)
        {
            Destroy(transform.root.gameObject);
        }
    }

    private void FindTargets()
    {
        if (baseIsland == null)
        {
            return;
        }
        target = null;
        float minDistance = Mathf.Infinity;
        foreach (Collider2D collider in targetList)
        {
            if (collider.CompareTag("Enemy"))
            {
                float distance = Vector2.Distance(collider.transform.position, baseIsland.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    target = collider.transform;
                }
            }
        }
    }

    private void Attack()
    {
        foreach (Collider2D target in targetList)
        {
            if (target.CompareTag("Enemy"))
            {
                float distance = Vector2.Distance(target.transform.position, transform.position);

                float damageMultiplier = Mathf.Clamp01((rangeRadius - distance) / rangeRadius);
                damageMultiplier += Time.deltaTime * damageRate;

                float damage = maxDamage * damageMultiplier;

                //EnemyHealth enemyHealth = target.GetComponent<EnemyHealth>();
                //if (enemyHealth != null)
                //{
                //    enemyHealth.TakeDamage(damage);
                //}
                Debug.Log("Skada enemy");
            }
            
        }

        //flameParticles.Play();
    }

    // Set timer when placing the turret
    public void ToggleLifeTime(bool toggle)
    {
        if (toggle == true)
        {
            currentLifeTime = maxLifeTime;
            lifeTimeActive = true;
        }
        else
        {
            lifeTimeActive = false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, rangeRadius);
    }
}

