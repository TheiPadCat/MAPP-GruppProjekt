using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public float fireRate;

    [SerializeField] Transform target;
    [SerializeField] ContactFilter2D contactFilter;
    [SerializeField] Transform baseIsland;
    [SerializeField] float rangeRadius;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] LayerMask layerMask;
    [SerializeField] float turnSpeed;

    private CircleCollider2D scanArea;
    private List<Collider2D> targetList = new List<Collider2D>();
    private ContactFilter2D contractFilter;
    private ParticleSystem particles;
    private float fireCoolDown;

    // Start is called before the first frame update
    void Start()
    {
        particles = GetComponentInChildren<ParticleSystem>();
        contactFilter.useLayerMask = true;
        contactFilter.layerMask = layerMask;
        scanArea = GetComponent<CircleCollider2D>();
        scanArea.radius = rangeRadius;

        fireCoolDown = 0;
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
            transform.right = direction;

        }

        if (fireCoolDown <= 0f)
        {
            Shoot();

            fireCoolDown = 1f / fireRate;
        }

        fireCoolDown -= Time.deltaTime;


    }
    private void FindTarget()
    {
        target = targetList[0].transform;
        for (int i = 1; i < targetList.Count; i++)
        {
            if (Vector2.Distance(target.transform.position, baseIsland.position) > Vector2.Distance(targetList[i].transform.position, baseIsland.position))
            {
                target = targetList[i].transform;
            }
        }
    }


    private void Shoot()
    {
        GameObject newBullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);

        newBullet.GetComponent<bulletScript>().direction = transform.right;
        particles.Emit(5);
    }



    
    private void OnDrawGizmos()
    {
        if (target != null)
        {
            Gizmos.DrawLine(transform.position, target.transform.position);
        }

    }

}
