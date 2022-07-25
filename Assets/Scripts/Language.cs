using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Language : Singleton<Language>
{
    [SerializeField] private LanguageDictionary _languageDictionary;
    private Dictionary<string, string> _dictionary;

    public override void Awake()
    {
        _dictionary = _languageDictionary.dictionary.ToDictionary(element => element.key, element => element.value);
    }

    public string this[string key]
    {
        get
        {
            if (_dictionary.ContainsKey(key))
                return _dictionary[key];
            else
                return key;
        }
    }
}
