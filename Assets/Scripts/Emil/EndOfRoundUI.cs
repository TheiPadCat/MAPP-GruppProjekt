using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EndOfRoundUI : MonoBehaviour {
    [SerializeField] private Button nextRoundBtn;
    [SerializeField] private Image panelImg;
    [SerializeField] TextMeshProUGUI endOfRoundText, roundAndKillText;
  
    // Start is called before the first frame update
    void Start() {
 
        nextRoundBtn.onClick.AddListener(delegate { RoundManager.RoundBegin.Invoke(RoundManager.Instance.RoundNumber); Time.timeScale = 1; });
        RoundManager.RoundEnd += OnRoundEnd;
        RoundManager.RoundBegin += OnRoundStart;
    }

    private void Update() {
     
        roundAndKillText.text = $"    {RoundManager.Instance.RoundNumber} \n{RoundManager.Instance.KillsThisRound} / {RoundManager.Instance.KillsRequired}";
    }

    private void OnRoundStart(int number) {
        panelImg.enabled = false;
        for (int i = 0; i < transform.childCount; i++) transform.GetChild(i).gameObject.SetActive(false);
    }

    private void OnRoundEnd(int number) {
        Time.timeScale = 0;
        endOfRoundText.text = $"Round {number} Cleared!" + "Round Cleared!";
        panelImg.enabled = true;
        for (int i = 0; i < transform.childCount; i++) transform.GetChild(i).gameObject.SetActive(true);
    }

    private void OnDestroy() {
        RoundManager.RoundEnd -= OnRoundEnd;
        RoundManager.RoundBegin -= OnRoundStart;
    }
}
