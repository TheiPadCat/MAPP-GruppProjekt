using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flamethrower : MonoBehaviour
{
    public float fireRate;
    public float maxLifeTime;
    private float currentLifeTime;
    private bool lifeTimeActive;

    [SerializeField] Transform baseIsland;
    [SerializeField] float rangeRadius;
    [SerializeField] float maxDamage;
    [SerializeField] float damageRate;

    private CircleCollider2D scanArea;
    private List<Collider2D> targetList = new List<Collider2D>();
    private Dictionary<Collider2D, float> damageMap = new Dictionary<Collider2D, float>();

    [SerializeField] ParticleSystem flameParticles;

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
        scanArea.OverlapCollider(new ContactFilter2D(), targetList);

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

        // Sort targets by distance to base island
        targetList.Sort((a, b) => Vector2.Distance(a.transform.position, baseIsland.position).CompareTo(Vector2.Distance(b.transform.position, baseIsland.position)));

        damageMap.Clear();
        foreach (Collider2D target in targetList)
        {
            float distance = Vector2.Distance(target.transform.position, transform.position);
            float damage = maxDamage * Mathf.Clamp01((rangeRadius - distance) / rangeRadius);

            if (damageMap.ContainsKey(target))
            {
                damage += damageMap[target];
            }

            damageMap[target] = damage;
        }
    }

    private void Attack()
    {
        foreach (Collider2D target in targetList)
        {
            float distance = Vector2.Distance(target.transform.position, transform.position);
            float damage = damageMap[target];
            float damageMultiplier = Mathf.Clamp01((rangeRadius - distance) / rangeRadius);
            damageMultiplier += Time.deltaTime * damageRate;

            Debug.Log("skada enemy");
            //EnemyHealth enemyHealth = target.GetComponent<EnemyHealth>();
            //if (enemyHealth != null)
            //{
            //   enemyHealth.TakeDamage(damage * damageMultiplier);
            //}
        }

        flameParticles.Play();
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

