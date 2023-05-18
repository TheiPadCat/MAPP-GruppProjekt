using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

/* 
TODO:
- make separate scripts for each enemy type
- Make main enemy controller and editor, as for spawner
- Behaviour tree?
 */
public class EnemyScript : MonoBehaviour, IEnemy, ISpawnable {

    [SerializeField] float maxHealth;
    private float currentHealth;
    private LootDrop drop;
    [SerializeField] ParticleSystem spawnParticles;

    [SerializeField] private ParticleSystem deathSplat;
    [SerializeField] private ParticleSystem deathStain;
    [SerializeField] private ParticleSystem deathTest;
    // Start is called before the first frame update
    void Start() {
        currentHealth = maxHealth;
        drop = GetComponent<LootDrop>();
    }

    // Update is called once per frame
    void Update() {

    }

    // Is called by the Spawner script on spawn
    public void Spawn() {
        // this could probs be used for spawn animations, events etc (if we want any)
        spawnParticles.Play();
        //transform.root.GetComponent<AIDestinationSetter>().target = Island.Instance.transform;

    }

    // temp
    public void Attack() {
        print("oooh I'm so scary I'm attacking you!");
    }

    private void OnCollisionEnter2D(Collision2D collision) { // this is a bit OP
        if (collision.gameObject.CompareTag("Player")) Die();
    }


    public void Die() {
        IEnemy.Death.Invoke(GetType(), transform.root.gameObject);

        deathSplat.transform.parent = null;
        deathStain.transform.parent = null;
        deathTest.transform.parent = null;
        deathSplat.Play();
        deathStain.Play();
        deathTest.Play();

        if (drop) drop.DropLoot();
        Destroy(transform.root.gameObject);
    }

    public void TakeDamage(float dmg) {
        currentHealth -= dmg;
        if (currentHealth <= 0) Die();



        if (GetComponent<DamageEffects>() != null) {
            GetComponent<DamageEffects>().PlayFlash();
        }

    }
}
