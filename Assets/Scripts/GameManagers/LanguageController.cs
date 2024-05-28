using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;


public enum Languages
{
    Español=0,
    English=1,
}

[Serializable]
public class TextInLang
{
    public Languages _language;
    public string _Text;
}


[Serializable]
public class LangOptionsInText
{
    public TMP_Text textObject;
    public List<TextInLang> langsDictionary= new List<TextInLang>();
}
public class LanguageController : MonoBehaviour
{
    public static LanguageController Instance{ get; private set; }
    public Languages _lang;
    [SerializeField] private UIDocument uiController;
    public VisualElement _MainMenu;
    public VisualElement _SettingsMenu;
    public VisualElement _CreditsMenu;

    private void Start()
    {
        VisualElement root = uiController.rootVisualElement;
        _MainMenu = root.Q("MainMenu");
        _CreditsMenu = root.Q("CreditsMenu");
        _SettingsMenu = root.Q("SettingsMenu");
    }

    public void SetMainMenu()
    {
        VisualElement root = uiController.rootVisualElement;
        //Button loadGame = root.Q<Button>("LoadButton");
        _MainMenu.visible = true;
    }


    // Update is called once per frame
    public void UpdateLanguage(Languages lang)
    {
        _lang = lang;
        
    }
    private void Awake() 
    { 
        // If there is an instance, and it's not me, delete myself.
    
        if (Instance != null && Instance != this) 
        { 
            Destroy(this.gameObject); 
        } 
        else 
        {
            Instance = this; 
        } 
    }
}
