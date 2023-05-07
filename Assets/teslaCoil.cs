using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class teslaCoil : MonoBehaviour
{
    private Collider2D colliderArea;
    [SerializeField] ParticleSystem electricityParticles;
    // Start is called before the first frame update
    void Start()
    {

        colliderArea = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
