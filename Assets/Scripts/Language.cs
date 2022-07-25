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

    public string GetAbilityText(AbilitySchema ability)
    {
        if (ability == null) return string.Empty;

        var needPoints = ability.needPoints > 0 ? "○: " : "";
        if (ability.maxRange > 0)
        {
            return $"{needPoints}{this[ability.name]} {ability.values[0]}-{ability.values[1]}-{ability.values[2]}. {this["range"]} {ability.maxRange}";
        }
        else
        {
            return $"{needPoints}{this[ability.name]} {ability.values[0]}-{ability.values[1]}-{ability.values[2]}";
        }
    }
}
