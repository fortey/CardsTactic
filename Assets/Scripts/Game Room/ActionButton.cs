using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ActionButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private Button _button;
    public event Action<string> onClick;

    public void Initialize(AbilitySchema abilitySchema)
    {
        _text.text = GetText(abilitySchema);

        _button.onClick.RemoveAllListeners();
        _button.onClick.AddListener(() => onClick?.Invoke(abilitySchema.name));
    }

    private string GetText(AbilitySchema abilitySchema)
    {
        switch (abilitySchema.name)
        {
            case ("melee"):
                return $"Атака {abilitySchema.values[0]}-{abilitySchema.values[1]}-{abilitySchema.values[2]}";
            case ("shot"):
                return $"Выстрел {abilitySchema.values[2]}. Дальность {abilitySchema.values[1]}";
            default:
                return string.Empty;
        }
    }

    public void SetInteractable(bool interactable)
    {
        _button.interactable = interactable;
    }
}