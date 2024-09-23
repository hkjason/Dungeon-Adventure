using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class LanguageReader : MonoBehaviour
{
    public static LanguageReader _instance;
    public SystemLanguage currentLanguage;
    // Start is called before the first frame update
    private string[] fileData;
    private string[] languageKeys;
    private List<string> contentKeys = new List<string>();
    private List<string> content = new List<string>();
    public Dictionary<string, Dictionary<string, string>> languageDictionary = new Dictionary<string, Dictionary<string, string>>();


    //delegate
    public delegate void OnLanguageChange();
    public OnLanguageChange onLanguageChange;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Debug.Log("language reader two singleton failure");
        }
    }

    void Start()
    {
        /* CSV檔案路徑 */
        string filePath = Application.streamingAssetsPath + "/DungeonGameContent.csv";
        /* 讀取CSV檔案，一行行讀取 */
        fileData = File.ReadAllLines(filePath);

        languageKeys = fileData[0].Split(',');
        for (int j = 1; j < fileData.Length; j++)
        {
            //mainmenu etc.
            contentKeys.Add(fileData[j].Split(',')[0]);
        }
        
        for (int i = 1; i < languageKeys.Length; i++)
        {
            //language key(2) * contentkeys(4)
            Dictionary<string, string> contentDict = new Dictionary<string, string>();
            for (int j = 0; j < contentKeys.Count; j++)
            {
                content = fileData[j+1].Split(',').ToList<string>();
                contentDict.Add(contentKeys[j], content[i]);
            }
            languageDictionary.Add(languageKeys[i], contentDict);
        }
    }

    public void LanguageChange()
    {
        if (currentLanguage == SystemLanguage.English)
        {
            currentLanguage = SystemLanguage.ChineseTraditional;
        }
        else
        {
            currentLanguage = SystemLanguage.English;
        }
        onLanguageChange();
    }
}
