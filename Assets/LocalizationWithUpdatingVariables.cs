using System;
using System.Buffers.Text;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;

public class LocalizationWithUpdatingVariables : MonoBehaviour
{
    [SerializeField] private LocalizedString localStringHp;
    [SerializeField] private TextMeshProUGUI textComponent;
    public string baseHp;
    private GameObject island;
 


    void Start()
    {
        island = GameObject.FindWithTag("Island");

        // Kunde inte bara sl�nga in baseHp, pga det m�ste vara av typen objekt. Skapade en lista ist�llet. "Cannot implicitly convert type 'string' to 'System.Collections.Generic.IList<object>"
        localStringHp.Arguments = new List<object> { baseHp };


        //Om health �ndras : uppdatera texten (eventet �r "StringChanged" fr�n LocalizedString-klassen).Varje g�ng texten �ndras kommer UpdateHpText k�ras
        localStringHp.StringChanged += UpdateHpText;
    }
    private void Update()
    {
        if (island != null)
        {
            baseHp = island.GetComponent<Island>().GetCurrentHealthAsString();

        }
        else Debug.Log("Kan inte hitta Island!");
    }
    private void UpdateHpText(string value)
    {
        textComponent.text = value;
    }

    // G�ra s� att updatetext inte l�ngre kallas om objektet �r avaktiverat.
    private void OnDisable()
    {
        localStringHp.StringChanged -= UpdateHpText;
    }
}

