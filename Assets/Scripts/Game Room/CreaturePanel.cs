using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Text;

public class CreaturePanel : MonoBehaviour
{
    [SerializeField] private GameRoomController _gameRoomController;
    [SerializeField] private ActionButton[] _actionButtons;
    [SerializeField] private CreatureInfo _creatureInfo;

    private Creature _creature;
    private bool _isEnemyTurn;

    private void Start()
    {
        foreach (var button in _actionButtons)
        {
            button.onClick += _gameRoomController.OnAbilityClick;
        }

        _gameRoomController.OnCreatureSelected += OnCreatureSelected;
        _gameRoomController.OnSelectedCreatureChanged += OnCreatureSelected;
        _gameRoomController.OnTurnChanged += OnTurnChanged;
    }
    private void OnCreatureSelected(Creature creature)
    {
        _creature = creature;
        if (creature == null)
        {
            _creatureInfo.gameObject.SetActive(false);
        }
        else
        {
            _creatureInfo.gameObject.SetActive(true);
            _creatureInfo.UpdateInfo(creature);
        }

        UpdateActions();
    }

    private void UpdateActions()
    {
        if (_creature)
            SetInteractableAllButtons(_creature.Schema.active);

        var needPoints = false;
        for (int i = 0; i < _actionButtons.Length; i++)
        {
            var button = _actionButtons[i];
            if (_creature != null && !_creature.IsEnemy && i < _creature.Abilities.Count)
            {
                button.Initialize(_creature.Abilities[i]);
                button.gameObject.SetActive(true);

                if (_creature.Abilities[i].needPoints > 0)
                {
                    needPoints = true;
                    if (_creature.Schema.points < _creature.Abilities[i].needPoints) button.SetInteractable(false);
                }
            }
            else
            {
                button.gameObject.SetActive(false);
            }
        }

        if (_creature != null && !_creature.IsEnemy)
        {
            if (needPoints)
            {
                var takePointsButton = _actionButtons[_creature.Abilities.Count];
                takePointsButton.Initialize("take_points");
                takePointsButton.gameObject.SetActive(true);
            }

            var button = _actionButtons[_creature.Abilities.Count + 1];
            button.Initialize("defense");
            button.gameObject.SetActive(true);

            button = _actionButtons[_creature.Abilities.Count + 2];
            button.Initialize("pass");
            button.gameObject.SetActive(true);

            if (_isEnemyTurn)
                SetInteractableAllButtons(false);
        }
    }

    private void OnTurnChanged(bool isEnemyTurn)
    {
        _isEnemyTurn = isEnemyTurn;
        UpdateActions();
    }

    private void SetInteractableAllButtons(bool interactable)
    {
        foreach (var button in _actionButtons)
        {
            button.SetInteractable(interactable);
        }
    }

    private void OnDestroy()
    {
        _gameRoomController.OnCreatureSelected -= OnCreatureSelected;
        _gameRoomController.OnSelectedCreatureChanged -= OnCreatureSelected;
        _gameRoomController.OnTurnChanged -= OnTurnChanged;
    }
}
