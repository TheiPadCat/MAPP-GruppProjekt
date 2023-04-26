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
    private int health;
    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        UpdateHealthText();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    
    private void OnCollisionEnter2D(Collision2D collision)
    {
       
        if(collision.gameObject.CompareTag("Enemy"))
        {
            health -= 1;

            Destroy(collision.gameObject.transform.root.gameObject);
      

            UpdateHealthText();

            if (health <= 0)
            {
                LoseGame();
            }
        }
    }

    
    public void LoseGame()
    {
      

        GameOverPanel.SetActive(true);
        
    }

    
    private void UpdateHealthText()
    {
        healthText.text = "HP: " + health.ToString();
    }

}
