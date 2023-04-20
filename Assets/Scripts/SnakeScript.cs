using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeScript : MonoBehaviour
{


    public List<GameObject> trailList = new List<GameObject>();
    [SerializeField] float dampTime;
    [SerializeField] float followDistance;
    [SerializeField] GameObject pickParticles;

    [SerializeField] GameObject turretPrefab;

    private Vector3[] boatVelocity;
    // Start is called before the first frame update



    private void Awake()
    {
      
    }
    void Start()
    {
        pickParticles = GameObject.Find("PickParticles");
        Calibrate();

    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space))
        {
            ReleaseBoat();
        }


        for (int i = 0; i < trailList.Count; i++)
        {
            Vector3 direction;
            Vector3 targetPosition;


            if (i == 0)
            {
                direction = trailList[i].transform.position - transform.position;
                targetPosition = transform.position + direction.normalized * followDistance;

            }
            else
            {
                direction = trailList[i].transform.position - trailList[i - 1].transform.position;
                targetPosition = trailList[i - 1].transform.position + direction.normalized * followDistance;

            }

            trailList[i].transform.right = direction;
            trailList[i].transform.position = Vector3.SmoothDamp(trailList[i].transform.position, targetPosition, ref boatVelocity[i], dampTime);


        }


    }


   public void ReleaseBoat()
    {
        if (trailList.Count > 0)
        {
            trailList[trailList.Count - 1].GetComponentInChildren<Turret>().ToggleLifeTime(true);
            trailList.Remove(trailList[trailList.Count - 1]);


        }

    }
    private void AddBoat(GameObject boat)
    {
        trailList.Add(boat);
    }

    public void Calibrate()
    {
        boatVelocity = new Vector3[trailList.Count];
    }



    private void OnDrawGizmos()
    {
        /*

        for (int i = 0; i < trailList.Count; i++)
        {
            Vector3 direction;
            Vector3 targetPosition;


            if (i == 0)
            {
                direction = trailList[i].transform.position - transform.position;
                targetPosition = transform.position + direction.normalized * followDistance;


            }
            else
            {
                direction = trailList[i].transform.position - trailList[i - 1].transform.position;
                targetPosition = trailList[i - 1].transform.position + direction.normalized * followDistance;



            }

            Gizmos.DrawLine(trailList[i].transform.position, targetPosition);
        }

        */
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Turret"))
        {
            if(!trailList.Contains(collision.gameObject))
            {
                trailList.Add(collision.gameObject);
                Calibrate();
                collision.gameObject.GetComponentInChildren<Turret>().ToggleLifeTime(false);

                PlayPickParticles(collision.transform.position);
            }
           
        }
     
        else if(collision.gameObject.CompareTag("Supply"))
        {
            GameObject newTurret = collision.gameObject.GetComponentInChildren<Package>().Unpack();
           trailList.Add(newTurret);
            Calibrate();
            PlayPickParticles(collision.transform.position);
            Destroy(collision.gameObject);
        
        }
    
    }

   

    private void PlayPickParticles(Vector3 pos)
    {
        pickParticles.transform.position = pos;
        pickParticles.GetComponent<ParticleSystem>().Emit(10);
    }
}
