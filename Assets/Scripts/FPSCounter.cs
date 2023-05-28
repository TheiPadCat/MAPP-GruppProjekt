using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class FPSCounter : MonoBehaviour
{
 //   public Text fpsText;
    public TMP_Text fpsText;
 
    private float updateInterval = 0.2f;

    IEnumerator Start()
    {
        while (true)
        {
            float fps = 1.0f / Time.deltaTime;
            fpsText.text = "FPS: " + Mathf.Round(fps).ToString();
            yield return new WaitForSeconds(updateInterval);
        }
    }




}
