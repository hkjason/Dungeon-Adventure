using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LanguageSetter : MonoBehaviour
{
    public string contentKey;

    public void Start()
    {
        LanguageReader._instance.onLanguageChange += LanguageChange;
    }
    public void LanguageChange()
    {
        if (LanguageReader._instance.languageDictionary.ContainsKey(LanguageReader._instance.currentLanguage.ToString()))
        {
            if (LanguageReader._instance.languageDictionary[LanguageReader._instance.currentLanguage.ToString()].ContainsKey(contentKey))
            {
                this.GetComponent<Text>().text = LanguageReader._instance.languageDictionary[LanguageReader._instance.currentLanguage.ToString()][contentKey];
            }
        }
    }

    public void OnValidate()
    {
        //contentKey = this.GetComponent<Text>().text;
        contentKey = this.gameObject.name;
    }
}
