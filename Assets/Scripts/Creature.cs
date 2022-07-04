using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Creature : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private TextMeshProUGUI _nameLabel;
    public string ID { get; private set; }

    public void Initialize(CreatureSchema schema)
    {
        _nameLabel.text = schema.name;
        _image.sprite = Global.Instance.CardSprites[schema.name];
        ID = schema.id;
    }
}
