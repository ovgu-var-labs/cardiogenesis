using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsHandler : MonoBehaviour
{
    /*
    int selected;
    string SELECTED_LANGUAGE = "SelectedLocale";
    bool changing;
    public TMP_Dropdown languageDropdown;
    private bool initialized;
    IEnumerator Start()
    {
        languageDropdown.options.Clear();

        // Generate list of available Locales
        yield return LocalizationSettings.InitializationOperation;

        var options = new List<TMP_Dropdown.OptionData>();
        for (var i = 0; i < LocalizationSettings.AvailableLocales.Locales.Count; i++)
        {
            var locale = LocalizationSettings.AvailableLocales.Locales[i];
            options.Add(new TMP_Dropdown.OptionData(locale.LocaleName));
        }
        languageDropdown.options = options;
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[selected];

        //set Listener
        languageDropdown.onValueChanged.AddListener(LocaleSelected);

        //set language
        selected = PlayerPrefs.GetInt(SELECTED_LANGUAGE,0);
        languageDropdown.value = selected;

        //Debug.Log(selected);

       


    }

    private void LocaleSelected(int localeID)
    {
        changing = true;
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[localeID];
        PlayerPrefs.SetInt(SELECTED_LANGUAGE, localeID);
        changing = false;
    }
    */
   
}
