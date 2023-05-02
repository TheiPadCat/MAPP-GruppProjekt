using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;



public class Island : MonoBehaviour {
    [SerializeField] GameObject GameOverPanel;
    [SerializeField] TMP_Text healthText;
    public int maxHealth;
    private int health;
    public static Island Instance;

    void Start() {
        Instance ??= this;
        health = maxHealth;
        UpdateHealthText();
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Enemy")) {
            health -= 1;

            collision.gameObject.GetComponent<EnemyScript>().Die();
            Debug.Log("DK ODODODODODO");
            //Destroy(collision.gameObject.transform.root.gameObject);

            UpdateHealthText();
            if (health <= 0) LoseGame();
        }
    }


    public void LoseGame() { GameOverPanel.SetActive(true);
        Time.timeScale = 0f;
    }
   
    private void UpdateHealthText() { healthText.text = "Base HP: " + health.ToString(); }
    private void OnDestroy() { Instance = null; }
    private void OnEnable() { Instance ??= this; }
    private void OnDisable() { Instance = null; }

}
