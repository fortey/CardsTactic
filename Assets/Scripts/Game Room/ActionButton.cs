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
        _text.text = Language.Instance.GetAbilityText(abilitySchema);

        _button.onClick.RemoveAllListeners();

        _button.onClick.AddListener(() => onClick?.Invoke(abilitySchema.name));
    }

    public void Initialize(string name)
    {
        _text.text = Language.Instance[name];

        _button.onClick.RemoveAllListeners();

        _button.onClick.AddListener(() => onClick?.Invoke(name));
    }

    public void SetInteractable(bool interactable)
    {
        _button.interactable = interactable;

        _image.color = interactable ? Color.white : _notActiveColor;
    }
}
