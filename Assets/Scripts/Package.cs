using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Package : MonoBehaviour, ISpawnable {

    public List<GameObject> turretList = new List<GameObject>();
    [SerializeField] ParticleSystem pickupParticle;

    private int Randomize() {
        return Random.Range(0, turretList.Count);
    }


    public GameObject Unpack() {
        GameObject newTurret = Instantiate(turretList[Randomize()], transform.position, Quaternion.identity);
        pickupParticle.transform.parent = null;
        pickupParticle.Play();
        return newTurret;
       
    }

    public void Spawn() {
        // spawn animations, or anything else that we want to happen on spawn go here
    }

    public void Despawn() { Destroy(gameObject); print("Package despawned!"); }
}
