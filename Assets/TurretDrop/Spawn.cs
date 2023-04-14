using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    [SerializeField] private GameObject prefab;
    [SerializeField] private Vector2 spawnPosition;
    [SerializeField] private bool random;

    public void SpawnPrefab()
    {
        if(random)
        {
            
            float x = Random.Range(-5,5);
            float y = Random.Range(-2, 2);
            GameObject turret = Instantiate(prefab,new Vector2(x,y), Quaternion.identity);
            StartCoroutine(WaitBeforeDelete(2, turret));
        }
        else
        {
            
           GameObject turret = Instantiate(prefab, spawnPosition, Quaternion.identity);
            StartCoroutine(WaitBeforeDelete(2, turret));
        }

    }
    IEnumerator WaitBeforeDelete(int seconds, Object instantiatedTurret)
    {
        yield return new WaitForSeconds(seconds);
        Destroy(instantiatedTurret);

    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            print("Spawnar turret");
            SpawnPrefab();

        }
    }
}
