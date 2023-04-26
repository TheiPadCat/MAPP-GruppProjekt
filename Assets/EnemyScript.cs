using System.Collections;
using System.Collections.Generic;
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
    // Start is called before the first frame update
    void Start() {
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update() {

    }

    public void Spawn() {
        // this could probs be used for spawn animations, events etc (if we want any)
    }

    // temp
    public void Attack() {
        print("oooh I'm so scary I'm attacking you!");
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            Debug.Log("Collision");
            Destroy(transform.root.gameObject);
        }
    }


    private void Die()
    {
        if(GetComponent<LootDrop>() != null)
        {
            GetComponent<LootDrop>().DropLoot();
        }
        Destroy(transform.root.gameObject);
    }

    public void TakeDamage(float dmg) {
        currentHealth -= dmg;
        if (currentHealth <= 0) {
            Die();
        }
    }
}
