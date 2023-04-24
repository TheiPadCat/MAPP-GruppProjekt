using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flames : MonoBehaviour
{
    public float fireRate;
    public float maxLifeTime;
    private float currentLifeTime;
    private bool lifeTimeActive;

    private Transform target;
    [SerializeField] ContactFilter2D contactFilter;
    [SerializeField] Transform baseIsland;
    [SerializeField] float rangeRadius;
    [SerializeField] GameObject bulletPrefab;

    [SerializeField] float turnSpeed;



    private CircleCollider2D scanArea;
    private List<Collider2D> targetList = new List<Collider2D>();
    [SerializeField] ParticleSystem sparkParticles;
    [SerializeField] ParticleSystem smokeParticles;
    [SerializeField] ParticleSystem ExplosionParticles;
    private float fireCoolDown;

    [SerializeField] float maxDamage;
    [SerializeField] float damageRate;



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
            FindTarget();

        }
        else
        {
            target = null;
        }


        if (target != null)
        {
            Vector3 direction = target.transform.position - transform.position;

            //  transform.right = Vector3.Lerp(transform.right, direction, Time.deltaTime * turnSpeed);

            transform.right = new Vector3(direction.x, direction.y, direction.z);
        }

        if (fireCoolDown <= 0f && target != null)
        {
            Shoot();

            fireCoolDown = 1f / fireRate;
        }

        fireCoolDown -= Time.deltaTime;

        if (lifeTimeActive)
        {
            currentLifeTime -= Time.deltaTime;

            if (currentLifeTime <= 0f)
            {
                Destroy(transform.root.gameObject);
            }

        }


    }
    private void FindTarget()
    {
        if (baseIsland == null)
        {
            return;
        }
        target = targetList[0].transform;
        for (int i = 1; i < targetList.Count; i++)
        {

            //Siktar p? fienden n?rmast basen
            if (Vector2.Distance(target.transform.position, baseIsland.position) > Vector2.Distance(targetList[i].transform.position, baseIsland.position))
            {
                target = targetList[i].transform;
            }
        }
    }


    private void Shoot()
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
                target.GetComponent<EnemyScript>().TakeDamage(damage);
                //    enemyHealth.TakeDamage(damage);
                //}
                Debug.Log("Skada enemy");
            }

        }

    }

    //S?tter p? timer n?r man l?gger ut den




    private void OnDrawGizmos()
    {
        if (target != null)
        {
            Gizmos.DrawLine(transform.position, target.transform.position);



        }

    }


}
