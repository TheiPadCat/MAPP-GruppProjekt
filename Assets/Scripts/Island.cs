using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using JetBrains.Annotations;

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

    [SerializeField] GameObject changeCharacterButton;

    void Start() {
        volume.profile.TryGet(out chromaticAberration);
        Instance ??= this;
        health = PlayerPrefs.GetInt("BaseHealth", maxHealth);
        UpdateHealthText();
        
         
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Enemy")) {
            health -= 1;

            GetComponent<AudioSource>().Play();
            //Literally en rad kod som gör att mobilen vibrerar, inte anpassad till olika mobiler men vi får testa och se om det blir whack med vissa //Lova : )
            //KOMMENTERAR UT DEN FÖR ATT JAG HAR PANIK O DEN INTE FUNKAR PÅ WINDOWS DEN FINNS I MOBILBUILDEN /JEOL
            //Handheld.Vibrate();

            collision.gameObject.GetComponent<EnemyScript>().Die();

            //Destroy(collision.gameObject.transform.root.gameObject);

            chromaticAberration.intensity.value += 0.1f;
            // globalVolume.GetComponent<ChromaticAberration>().intensity = (health / maxHealth)

            damageParticles.Emit(80);
           UpdateHealthText();

            CinemachineCameraShake.Instance.ShakeCamera(10f, .2f);
            //om basens nuvarande health modulo 5 är noll aka var femte minus i health och om basen inte har maxhealth
            /*
            if (health % 5 == 0 && health != maxHealth) 
            {
                CinemachineCameraShake.Instance.ShakeCamera(5f, .1f);
            }**/
            if (health <= 0) LoseGame();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            changeCharacterButton.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            changeCharacterButton.SetActive(false);
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
    public string GetCurrentHealthAsString()
    {
        return health.ToString();
    }

     private void UpdateHealthText() {
        healthText.text = health.ToString();
    }
    private void OnDestroy() { Instance = null; }
    private void OnEnable() { Instance ??= this; }
    private void OnDisable() { Instance = null; }

}
