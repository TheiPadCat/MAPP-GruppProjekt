using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public float fireRate;
    public float maxLifeTime;
    private float currentLifeTime;
    private bool lifeTimeActive;

    [SerializeField] Transform target;
    [SerializeField] ContactFilter2D contactFilter;
    [SerializeField] Transform baseIsland;
    [SerializeField] float rangeRadius;
    [SerializeField] GameObject bulletPrefab;

    [SerializeField] float turnSpeed;



    private CircleCollider2D scanArea;
    private List<Collider2D> targetList = new List<Collider2D>();
    [SerializeField] ParticleSystem sparkParticles;
    [SerializeField] ParticleSystem smokeParticles;
    private float fireCoolDown;

    // Start is called before the first frame update
    void Start()
    {
       

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
            //   transform.right = direction;
            transform.right = Vector3.Lerp(transform.right, direction, Time.deltaTime * turnSpeed);
           

        }

        if (fireCoolDown <= 0f && target != null)
        {
            Shoot();

            fireCoolDown = 1f / fireRate;
        }

        fireCoolDown -= Time.deltaTime;

        if(lifeTimeActive)
        {
            currentLifeTime -= Time.deltaTime;
        }

        if(currentLifeTime <= 0f && lifeTimeActive)
        {
            Destroy(transform.root.gameObject);
        }

    }
    private void FindTarget()
    {
        target = targetList[0].transform;
        for (int i = 1; i < targetList.Count; i++)
        {

            //Siktar på fienden närmast basen
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
        sparkParticles.Emit(5);
        smokeParticles.Emit(4);
    }

    //Sätter på timer när man lägger ut den
    public void ToggleLifeTime(bool toggle)
    {
        if(toggle == true)
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
        if (target != null)
        {
            Gizmos.DrawLine(transform.position, target.transform.position);
        }

    }

}
