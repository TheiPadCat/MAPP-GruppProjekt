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
    public int health;
    public static Island Instance;

    public Volume volume;
    private ChromaticAberration chromaticAberration;

    ChromaticAberration postProcessChromaticAberration;

    [SerializeField] ParticleSystem damageParticles;

    void Start() {
        volume.profile.TryGet(out chromaticAberration);
        Instance ??= this;
        health = PlayerPrefs.GetInt("BaseHealth", maxHealth);
        UpdateHealthText();

         
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Enemy")) {
            health -= 1;

            collision.gameObject.GetComponent<EnemyScript>().Die();

            //Destroy(collision.gameObject.transform.root.gameObject);

            chromaticAberration.intensity.value += 0.1f;
            // globalVolume.GetComponent<ChromaticAberration>().intensity = (health / maxHealth)

            damageParticles.Emit(80);
            UpdateHealthText();
            if (health % 5 == 0 && health != maxHealth) 
            {
                CinemachineCameraShake.Instance.ShakeCamera(5f, .1f);
            }
            if (health <= 0) LoseGame();
        }
    }

    public void EndRound()
    {
        PlayerPrefs.SetInt("BaseHealth", health);
        PlayerPrefs.Save();
    }

    public void LoseGame() { GameOverPanel.SetActive(true);
        Time.timeScale = 0f;
    }
   
    private void UpdateHealthText() { healthText.text = "Base HP: " + health.ToString(); }
    private void OnDestroy() { Instance = null; }
    private void OnEnable() { Instance ??= this; }
    private void OnDisable() { Instance = null; }

}
