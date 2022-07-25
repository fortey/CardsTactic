using UnityEngine;

[CreateAssetMenu(fileName = "Language", menuName = "Cards/New Language", order = 0)]
public class LanguageDictionary : ScriptableObject
{
    public DictionaryElement[] dictionary;
}

[System.Serializable]
public struct DictionaryElement
{
    public string key;
    public string value;
}
