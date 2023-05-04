using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootDrop : MonoBehaviour
{
    public int lootAmount;
    public GameObject lootObject;
    public float launchForce;


    public void DropLoot()
    {
        for(int i = 0; i < lootAmount; i++)
        {
           GameObject loot = Instantiate(lootObject, transform.position, Quaternion.identity);
            loot.GetComponent<Rigidbody2D>().AddForce(Random.insideUnitCircle * launchForce, ForceMode2D.Impulse);
        }
    }
}
