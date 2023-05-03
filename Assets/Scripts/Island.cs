using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;


public class Island : MonoBehaviour {
    [SerializeField] GameObject GameOverPanel;
    [SerializeField] TMP_Text healthText;
    public int maxHealth;
    private int health;
    public static Island Instance;

   public Volume volume;
    private ChromaticAberration chromaticAberration;
     
    ChromaticAberration postProcessChromaticAberration;

    void Start() {
        volume.profile.TryGet(out chromaticAberration);
        Instance ??= this;
        health = maxHealth;
        UpdateHealthText();

         
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Enemy")) {
            health -= 1;

            collision.gameObject.GetComponent<EnemyScript>().Die();

            //Destroy(collision.gameObject.transform.root.gameObject);

            chromaticAberration.intensity.value += 0.1f;
            // globalVolume.GetComponent<ChromaticAberration>().intensity = (health / maxHealth)

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
