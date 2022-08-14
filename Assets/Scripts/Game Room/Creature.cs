using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Creature : MonoBehaviour
{
    public event System.Action<Creature> OnChange;
    [SerializeField] private Image _image;
    [SerializeField] private TextMeshProUGUI _nameLabel;
    [SerializeField] private TextMeshProUGUI _healthLabel;
    [SerializeField] private TextMeshProUGUI _stepsLabel;
    [SerializeField] private Image _targetImage;
    [SerializeField] private GameObject _clock;
    [SerializeField] private GameObject _defense;
    [SerializeField] private GameObject _points;
    [SerializeField] private TextMeshProUGUI _pointsCount;
    [SerializeField] private Animator _animator;

    [Header("Attributes")]
    [SerializeField] private GameObject _shotProtection;
    [SerializeField] private GameObject _regeneration;
    [SerializeField] private GameObject _poisoning;

    public CreatureSchema Schema;
    public string ID { get; private set; }
    public string Owner { get; private set; }

    public List<AbilitySchema> Abilities { get; private set; } = new List<AbilitySchema>();
    public bool IsEnemy { get; private set; }
    private IEnumerator _waitForUpdate;

    public bool Active { get; private set; }
    private readonly int _anim_isWalk = Animator.StringToHash("isWalk");

    private void Start()
    {
        _waitForUpdate = new WaitForUpdate();
    }

    public void Initialize(CreatureSchema schema, bool isEnemy)
    {
        Schema = schema;
        _nameLabel.text = schema.name;
        _image.sprite = Global.Instance.CardSprites[schema.name].sprite;
        //_animator.runtimeAnimatorController = Global.Instance.CardSprites[schema.name].animator;

        var animatorOverrideController = Global.Instance.CardSprites[schema.name].animator;
        _animator.runtimeAnimatorController = animatorOverrideController;

        var clipOverrides = new List<KeyValuePair<AnimationClip, AnimationClip>>(animatorOverrideController.overridesCount);
        animatorOverrideController.GetOverrides(clipOverrides);
        animatorOverrideController.ApplyOverrides(clipOverrides);

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

        for (int i = 0; i < schema.attributes.Count; i++)
        {
            switch (schema.attributes[i])
            {
                case ("shot_protection"):
                    _shotProtection.SetActive(true);
                    break;
            }
        }

        for (int i = 0; i < schema.passiveAbilities.Count; i++)
        {
            switch (schema.passiveAbilities[i].name)
            {
                case ("regeneration"):
                    _regeneration.SetActive(true);
                    break;
                case ("poisoning"):
                    _poisoning.SetActive(true);
                    break;
            }
        }

        UpdateStats(schema);

        schema.OnChange += OnStateChanged;
        schema.passiveAbilities.OnAdd += OnAbilityAdd;
    }

    public void Move(Vector3 position)
    {
        _animator.SetBool(_anim_isWalk, true);
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
        _animator.SetBool(_anim_isWalk, false);
    }

    private void UpdateStats(CreatureSchema schema)
    {
        _healthLabel.text = schema.health.ToString();
        _stepsLabel.text = $"{schema.steps}/{schema.maxSteps}";

        _points.SetActive(schema.points > 0);
        if (schema.points > 0)
        {
            _pointsCount.text = schema.points.ToString();
        }
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
                    _stepsLabel.text = $"{changed.Value}/{Schema.maxSteps}";
                    break;
                case ("active"):
                    Active = (bool)changed.Value;
                    ShowClock(!Active);
                    break;
                case ("defense"):
                    _defense.SetActive((bool)changed.Value);
                    break;
                case ("points"):
                    var points = (float)changed.Value;
                    _points.SetActive(points > 0);
                    _pointsCount.text = points.ToString();
                    break;
                default:
                    print(changed.Field);
                    break;
            }
        }
        OnChange?.Invoke(this);
    }

    private void ShowPopupText(Color color, float value)
    {
        var popup = Global.Instance.PopupTextPool.Get();
        var text = value < 0 ? value.ToString() : $"+{value}";
        popup.GetComponent<PopupText>().Play(transform.position, color, text);
    }

    private void ShowClock(bool show)
    {
        _clock.SetActive(show);
    }

    private void OnAbilityAdd(int index, AbilitySchema ability)
    {
        if (ability.name == "poisoning") _poisoning.SetActive(true);
    }
}
