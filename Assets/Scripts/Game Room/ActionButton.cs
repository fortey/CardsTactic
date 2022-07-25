using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ActionButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private Button _button;
    [SerializeField] private Image _image;
    [SerializeField] private Color _notActiveColor;
    public event Action<string> onClick;

    public void Initialize(AbilitySchema abilitySchema)
    {
        _text.text = GetText(abilitySchema);

        _button.onClick.RemoveAllListeners();

        //var abilityName = abilitySchema != null ? abilitySchema.name : "pass";
        _button.onClick.AddListener(() => onClick?.Invoke(abilitySchema.name));
    }

    public void Initialize(string name)
    {
        _text.text = Language.Instance[name];

        _button.onClick.RemoveAllListeners();

        _button.onClick.AddListener(() => onClick?.Invoke(name));
    }

    private string GetText(AbilitySchema abilitySchema)
    {
        if (abilitySchema == null) return string.Empty;

        if (abilitySchema.maxRange > 0)
        {
            return $"{Language.Instance[abilitySchema.name]} {abilitySchema.values[0]}-{abilitySchema.values[1]}-{abilitySchema.values[2]}. {Language.Instance["range"]} {abilitySchema.maxRange}";
        }
        else
        {
            return $"{Language.Instance[abilitySchema.name]} {abilitySchema.values[0]}-{abilitySchema.values[1]}-{abilitySchema.values[2]}";
        }
    }

    public void SetInteractable(bool interactable)
    {
        _button.interactable = interactable;

        _image.color = interactable ? Color.white : _notActiveColor;
    }
}
