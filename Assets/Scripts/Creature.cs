using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Creature : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private TextMeshProUGUI _nameLabel;
    [SerializeField] private TextMeshProUGUI _healthLabel;
    [SerializeField] private Image _targetImage;
    public string ID { get; private set; }
    public string Owner { get; private set; }

    public List<AbilitySchema> Abilities { get; private set; } = new List<AbilitySchema>();
    public bool IsEnemy { get; private set; }
    private IEnumerator _waitForUpdate;

    private void Start()
    {
        _waitForUpdate = new WaitForUpdate();
    }

    public void Initialize(CreatureSchema schema, bool isEnemy)
    {
        _nameLabel.text = schema.name;
        _image.sprite = Global.Instance.CardSprites[schema.name];

        ID = schema.id;
        Owner = schema.owner;
        IsEnemy = isEnemy;

        for (int i = 0; i < schema.abilities.Count; i++)
        {
            Abilities.Add(schema.abilities[i]);
        }

        if (isEnemy) _image.material = Global.Instance.EnemyCardMaterial;
        else _image.material = Global.Instance.AllyCardMaterial;
        UpdateStats(schema);
    }

    public void Move(Vector3 position)
    {
        StartCoroutine(Moving(position));
    }

    private IEnumerator Moving(Vector3 position)
    {
        var time = 0.5f;
        var scale = 1 / time;
        var startPos = transform.position;
        while (time >= 0f)
        {
            time -= Time.deltaTime;
            transform.position = Vector3.Lerp(position, startPos, time * scale);
            yield return _waitForUpdate;
        }
    }

    private void UpdateStats(CreatureSchema schema)
    {
        _healthLabel.text = schema.health.ToString();
    }

    public void SetTarget(bool active)
    {
        _targetImage.gameObject.SetActive(active);
    }

    public void OnStateChanged(List<Colyseus.Schema.DataChange> changes)
    {
        foreach (var changed in changes)
        {
            switch (changed.Field)
            {
                case ("health"):
                    _healthLabel.text = changed.Value.ToString();
                    break;
                default:
                    print(changed.Field);
                    break;
            }
        }

    }
}
