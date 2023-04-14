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
    private List<Collider2D> targetList = new List<Collider2D>();
    // Start is called before the first frame update
    void Start()
    {
        scanArea = GetComponent<CircleCollider2D>();
        scanArea.radius = rangeRadius;
        InvokeRepeating("Shoot", 1, 1);
        
    }

    // Update is called once per frame
    void Update()
    {

        scanArea.OverlapCollider(contactFilter, targetList);

        if(targetList.Count > 0)
        {
           FindTarget();

        }
        else
        {
            target = null;
        }
    

        if(target != null)
        {
            Vector3 direction = target.transform.position - transform.position;

            Vector3 test = Quaternion.Euler(0, 0, 90) * direction;


            transform.right = direction;
           
       //    Quaternion rotationTarget = Quaternion.LookRotation(test, Vector3.forward);
            

        //    transform.rotation = rotationTarget;
            


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
        if(target != null)
        {
            Gizmos.DrawLine(transform.position, target.transform.position);
        }
       
    }




    private void Shoot()
    {
        GameObject newBullet = Instantiate(bulletPrefab,transform.position, Quaternion.identity);

        newBullet.GetComponent<bulletScript>().direction = transform.right;

        
        

    }
}
