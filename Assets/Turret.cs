using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{

    [SerializeField] Transform target;
    [SerializeField] ContactFilter2D contactFilter;
    [SerializeField] Transform baseIsland;
    [SerializeField] float rangeRadius;
    [SerializeField] GameObject bulletPrefab;
    private CircleCollider2D scanArea;
    public List<Collider2D> targetList = new List<Collider2D>();
    private ContactFilter2D contractFilter;
    [SerializeField] LayerMask layerMask;
    // Start is called before the first frame update
    void Start()
    {
        contactFilter.useLayerMask = true;
        contactFilter.layerMask = layerMask;
        scanArea = GetComponent<CircleCollider2D>();
        scanArea.radius = rangeRadius;
        InvokeRepeating("Shoot", 1, 1);

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

    private void OnDrawGizmos()
    {
        if (target != null)
        {
            Gizmos.DrawLine(transform.position, target.transform.position);
        }

    }


    private void Shoot()
    {
        GameObject newBullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);

        newBullet.GetComponent<bulletScript>().direction = transform.right;

    }
}
