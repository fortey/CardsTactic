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
    public string Owner { get; private set; }
    private bool _isEnemy;

    public void Initialize(CreatureSchema schema, bool isEnemy)
    {
        _nameLabel.text = schema.name;
        _image.sprite = Global.Instance.CardSprites[schema.name];

        ID = schema.id;
        Owner = schema.owner;
        _isEnemy = isEnemy;

        if (isEnemy) _image.material = Global.Instance.EnemyCardMaterial;
        else _image.material = Global.Instance.AllyCardMaterial;

    }
}
