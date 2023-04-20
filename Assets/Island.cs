using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;



public class Island : MonoBehaviour
{

    [SerializeField] GameObject GameOverPanel;
    [SerializeField] TMP_Text healthText;
    public int maxHealth;
    public int health;
    // Start is called before the first frame update
    void Start()
    {
        UpdateHealthText();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("test");
        if(collision.gameObject.CompareTag("Enemy"))
        {
            health -= 1;

            Destroy(collision.gameObject.transform.root.gameObject);
            Debug.Log("enemy hit");

            UpdateHealthText();

            if (health <= 0)
            {
                LoseGame();
            }
        }
    }


    public void LoseGame()
    {
        Debug.Log("YOU LOSE");

        GameOverPanel.SetActive(true);
        Time.timeScale = 0;
    }

    
    private void UpdateHealthText()
    {
        healthText.text = health.ToString();
    }

}
