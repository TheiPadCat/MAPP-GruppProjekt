using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SnakeScript : MonoBehaviour
{


    public List<GameObject> trailList = new List<GameObject>();
    public List<GameObject> releasedBoats = new List<GameObject>();
    [SerializeField] float dampTime;
    [SerializeField] float followDistance;
    //[SerializeField] GameObject pickParticles;

  


    [SerializeField] TMP_Text boatsText;
    public int maxBoats;
    private int currentBoats;

    private Vector3[] boatVelocity;
    // Start is called before the first frame update



    private void Awake()
    {
      
    }
    void Start()
    {
        UpdateBoatCounter();
        //pickParticles = GameObject.Find("PickParticles");
        Calibrate();

    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space))
        {
            ReleaseBoat();
        }


       


    }


    private void FixedUpdate()
    {
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

            //spara positionen av släppta båtar
            for(int j = 0; i < releasedBoats.Count; i++)
            {
                 Vector2 pos = releasedBoats[i].transform.position;
            }
        }
    }

    public void ReleaseBoat()
    {
        if (trailList.Count > 0)
        {
            //trailList[trailList.Count - 1].GetComponentInChildren<Turret>().ToggleLifeTime(true);
            GameObject releasedBoat = trailList[trailList.Count - 1];
            trailList[trailList.Count - 1].GetComponentInChildren<TimerController>().ToggleLifeTime(true);
            trailList.Remove(releasedBoat);
            releasedBoats.Add(releasedBoat);
            currentBoats--;

            UpdateBoatCounter();
        }

    }
    private void AddBoat()
    {
        
        currentBoats++;
        UpdateBoatCounter();
    }

    public void Calibrate()
    {
        boatVelocity = new Vector3[trailList.Count];
    }





    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(currentBoats < maxBoats)
        {

            if (collision.gameObject.CompareTag("Turret"))
            {
                Debug.Log("pick up");

                if (!trailList.Contains(collision.gameObject))
                {
                    trailList.Add(collision.gameObject);
                    Calibrate();

                    //KANSKE L�GGA TILLBAKA DET H�R IN VETE FAN M�STE FIXA S� FLAME THROWER HAR DEN OCKS�
                    // collision.gameObject.GetComponentInChildren<Turret>().ToggleLifeTime(false);
                    collision.gameObject.GetComponentInChildren<TimerController>().ToggleLifeTime(false);

                    PlayPickParticles(collision.transform.position);
                    AddBoat();


                }

            }

            else if (collision.gameObject.CompareTag("Supply"))
            {
                GameObject newTurret = collision.gameObject.GetComponentInChildren<Package>().Unpack();
                trailList.Add(newTurret);
                Calibrate();
                PlayPickParticles(collision.transform.position);
                Destroy(collision.gameObject);
                AddBoat();

            }

        }
    }
      

   

    private void PlayPickParticles(Vector3 pos)
    {
        //if(pickParticles != null)
        {
  //pickParticles.transform.position = pos;
       // pickParticles.GetComponent<ParticleSystem>().Emit(10);
        }
      
    }


    public void UpdateBoatCounter()
    {
        if(boatsText != null)
        {
 boatsText.text = currentBoats.ToString() + " / " + maxBoats.ToString();
        }
       
    }
}
