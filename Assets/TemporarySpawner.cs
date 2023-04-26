using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemporarySpawner : MonoBehaviour
{

    public GameObject enemy;
    public GameObject island;
    private float currentTime;
    public List<GameObject> gameObjects = new List<GameObject>();
    
    // Start is called before the first frame update
    void Start()
    {
        // InvokeRepeating("Spawn", 1, 8);
        currentTime = 5;

    }

    // Update is called once per frame
    void Update()
    {
        if(currentTime > 0)
        {
            currentTime -= Time.deltaTime;
        }
        else
        {
            Spawn();
            currentTime = Random.Range(1, 4);
        }
    }


    public void Spawn()
    {
        int random = Random.Range(0,gameObjects.Count);

        GameObject newEnemy = Instantiate(gameObjects[random],transform.position + new Vector3(Random.Range(-100,100), Random.Range(-100, 100),0),Quaternion.identity);
        newEnemy.GetComponent<AIDestinationSetter>().target = island.transform;
    }
}
