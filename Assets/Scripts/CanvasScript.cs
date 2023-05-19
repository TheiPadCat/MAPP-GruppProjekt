using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CanvasScript : MonoBehaviour
{
    [SerializeField] GameObject pausePanel;
    [SerializeField] GameObject characterPanel;
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;
        pausePanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1f;
    }
     

    public void BackToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }
    
    public void Pause()
    {
        pausePanel.SetActive(true);
        Time.timeScale = 0;
    }
    public void UnPause()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1;
    }

    public void ToggleCharacterSelect()
    {
        if(characterPanel.active == true)
        {
            characterPanel.SetActive(false);
            Time.timeScale = 1;
        }
        else
        {
            characterPanel.SetActive(true);
            Time.timeScale = 0;
        }

    }
    

    /*
    public void TogglePause(bool toggle)
    {
        if(toggle == true)
        {
            pausePanel.SetActive(true);
            Time.timeScale = 0;
        }
        else
        {
            pausePanel.SetActive(false);
            Time.timeScale = 1;
        }
    }
    */

}
