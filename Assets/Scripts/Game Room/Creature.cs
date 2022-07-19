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
    [SerializeField] private TextMeshProUGUI _stepsLabel;
    [SerializeField] private Image _targetImage;
    [SerializeField] private GameObject _clock;

    private CreatureSchema _schema;
    public string ID { get; private set; }
    public string Owner { get; private set; }

    public List<AbilitySchema> Abilities { get; private set; } = new List<AbilitySchema>();
    public bool IsEnemy { get; private set; }
    private IEnumerator _waitForUpdate;

    public bool Active { get; private set; }
    private void Start()
    {
        _waitForUpdate = new WaitForUpdate();
    }

    public void Initialize(CreatureSchema schema, bool isEnemy)
    {
        _schema = schema;
        _nameLabel.text = schema.name;
        _image.sprite = Global.Instance.CardSprites[schema.name];

        ID = schema.id;
        Owner = schema.owner;
        IsEnemy = isEnemy;
        Active = schema.active;

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
        _stepsLabel.text = $"{schema.steps}/{schema.maxSteps}";
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
                    var difference = (float)changed.Value - (float)changed.PreviousValue;
                    var color = difference < 0 ? Color.red : Color.green;
                    ShowPopupText(color, difference);
                    break;
                case ("steps"):
                    _stepsLabel.text = $"{changed.Value}/{_schema.maxSteps}";
                    break;
                case ("active"):
                    Active = (bool)changed.Value;
                    ShowClock(!Active);
                    break;
                default:
                    print(changed.Field);
                    break;
            }
        }

    }

    private void ShowPopupText(Color color, float value)
    {
        var popup = Global.Instance.PopupTextPool.Get();
        popup.GetComponent<PopupText>().Play(transform.position, color, value.ToString());
    }

    private void ShowClock(bool show)
    {
        _clock.SetActive(show);
    }
}
