using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Package : MonoBehaviour
{
 
    public List<GameObject> turretList = new List<GameObject>();
 

    private int Randomize()
    {
        return Random.Range(0, turretList.Count);
    }
    

    public GameObject Unpack()
    {
        GameObject newTurret = Instantiate(turretList[Randomize()], transform.position,Quaternion.identity);
    
        return newTurret;
        
       
    }
}
