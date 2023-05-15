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

        // Kunde inte bara slänga in baseHp, pga det måste vara av typen objekt. Skapade en lista istället. "Cannot implicitly convert type 'string' to 'System.Collections.Generic.IList<object>"
        localStringHp.Arguments = new List<object> { baseHp };


        //Om health ändras : uppdatera texten (eventet är "StringChanged" från LocalizedString-klassen).Varje gång texten ändras kommer UpdateHpText köras
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

    // Göra så att updatetext inte längre kallas om objektet är avaktiverat.
    private void OnDisable()
    {
        localStringHp.StringChanged -= UpdateHpText;
    }
}

