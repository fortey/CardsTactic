using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CreaturePanel : MonoBehaviour
{
    [SerializeField] private GameRoomController _gameRoomController;
    [SerializeField] private ActionButton[] _actionButtons;
    [SerializeField] private TextMeshProUGUI _name;
    [SerializeField] private TextMeshProUGUI _description;
    private void Start()
    {
        foreach (var button in _actionButtons)
        {
            button.onClick += _gameRoomController.OnAbilityClick;
        }

        _gameRoomController.OnCreatureSelected += OnCreatureSelected;
        _gameRoomController.OnTurnChanged += OnTurnChanged;
    }
    private void OnCreatureSelected(Creature creature)
    {
        UpdateInfo(creature);

        var needPoints = false;
        for (int i = 0; i < _actionButtons.Length; i++)
        {
            var button = _actionButtons[i];
            if (creature != null && !creature.IsEnemy && i < creature.Abilities.Count)
            {
                button.Initialize(creature.Abilities[i]);
                button.gameObject.SetActive(true);

                if (creature.Abilities[i].needPoints) needPoints = true;
            }
            else
            {
                button.gameObject.SetActive(false);
            }
        }

        if (creature != null && !creature.IsEnemy)
        {
            if (needPoints)
            {
                var takePointsButton = _actionButtons[creature.Abilities.Count];
                takePointsButton.Initialize("take_points", "Накопить очко действия");
                takePointsButton.gameObject.SetActive(true);
            }

            var button = _actionButtons[creature.Abilities.Count + 1];
            button.Initialize("defense", "Защита");
            button.gameObject.SetActive(true);

            button = _actionButtons[creature.Abilities.Count + 2];
            button.Initialize("pass", "Пропустить");
            button.gameObject.SetActive(true);
        }

    }
    private void OnTurnChanged(bool isEnemyTurn)
    {
        foreach (var button in _actionButtons)
        {
            button.SetInteractable(!isEnemyTurn);
        }
    }

    private void UpdateInfo(Creature creature)
    {
        _name.text = creature.name;

        //var builder = new StringBuilder();

    }
}
