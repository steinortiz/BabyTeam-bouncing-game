using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;


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
    public Languages _lang;
    [SerializeField] private List<LangOptionsInText> _textList = new List<LangOptionsInText>();
    // Update is called once per frame
    public void UpdateLanguage(Languages lang)
    {
        _lang = lang;
        foreach (var textInstance in _textList)
        {
            textInstance.textObject.text = textInstance.langsDictionary[(int)lang]._Text;
        }
    }
}
