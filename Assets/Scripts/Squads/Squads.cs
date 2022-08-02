using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Squads : MonoBehaviour
{
    [SerializeField] private Pool _squadItemsPool;
    private List<SquadItem> _squadItems = new List<SquadItem>();

    private UserCreature[] _creatures;
    [SerializeField] private Pool _creatureItemPool;
    [SerializeField] private CreatureListItem _creatureListItemPrefab;
    [SerializeField] private SquadBoard _squadBoard;

    [SerializeField] private SquadInventory _creatureInventory;
    [SerializeField] private GameObject _editMode;
    [SerializeField] private GameObject _squadListMode;

    private Squad _selectedSquad;

    private void OnEnable()
    {
        ClearItems();
        MyColyseusManager.Instance.GetSquads(OnSquads);
        MyColyseusManager.Instance.GetCreatures(OnCreatures);
    }

    private void OnSquads(Squad[] squads)
    {
        foreach (var squad in squads)
        {
            var squadItem = _squadItemsPool.Get().GetComponent<SquadItem>();
            squadItem.Initialize(squad);
            _squadItems.Add(squadItem);
        }
    }

    private void ClearItems()
    {
        foreach (var item in _squadItems)
        {
            _squadItemsPool.Push(item.gameObject);
        }
        _squadItems.Clear();
    }

    public void OnSquadSelected(SquadItem squadItem)
    {
        _selectedSquad = squadItem.Squard;

        for (int i = 0; i < squadItem.Squard.board.Length; i++)
        {
            var cell = _squadBoard[i];
            var creatureName = squadItem.Squard.board[i];
            if (creatureName == "") { }
            else
            {
                var creatureItem = _creatureItemPool.Get().GetComponent<CreatureListItem>();
                //Instantiate(_creatureListItemPrefab, cell.transform.position, Quaternion.identity);

                cell.SetItem(creatureItem.transform, creatureItem);
                creatureItem.gameObject.SetActive(true);////
                creatureItem.Initialize(creatureName);
                creatureItem.previousCell = cell.transform;
            }

        }
    }

    private void OnCreatures(UserCreature[] creatures)
    {
        _creatures = creatures;
    }

    public void EditSquad()
    {
        if (_selectedSquad != null)
        {
            _squadListMode.SetActive(false);
            _editMode.SetActive(true);
            _creatureInventory.gameObject.SetActive(true);
            _creatureInventory.Initialize(_squadBoard, _creatures, _creatureItemPool);
        }
    }
}
