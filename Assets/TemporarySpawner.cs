using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemporarySpawner : MonoBehaviour
{

    public GameObject enemy;
    public GameObject island;
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("Spawn", 1, 8);
        

    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void Spawn()
    {
        GameObject newEnemy = Instantiate(enemy,transform.position,Quaternion.identity);
        newEnemy.GetComponent<AIDestinationSetter>().target = island.transform;
    }
}
