using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //Physics2D.IgnoreLayerCollision(6, 6);
        Physics2D.IgnoreLayerCollision(6,10);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
