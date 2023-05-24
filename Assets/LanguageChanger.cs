using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization;

public class LanguageChanger : MonoBehaviour
{
  public void ToEnglish()
    {
       SetLanguage(LocalizationSettings.AvailableLocales.Locales[0]);   
    }
    public void ToSpanish()
    {
        SetLanguage(LocalizationSettings.AvailableLocales.Locales[1]);
    }

    public void ToSwedish()
    {
        SetLanguage(LocalizationSettings.AvailableLocales.Locales[2]);
    }
   

    private void SetLanguage(Locale language)
    {
        LocalizationSettings.SelectedLocale = language;
    }
}
