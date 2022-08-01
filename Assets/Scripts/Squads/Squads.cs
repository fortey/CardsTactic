using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Squads : MonoBehaviour
{
    [SerializeField] private Pool _squadItemsPool;
    private List<SquadItem> _squadItems = new List<SquadItem>();

    private UserCreature[] _creatures;

    private void OnEnable()
    {
        ClearItems();
        MyColyseusManager.Instance.GetSquads(OnSquads);
        MyColyseusManager.Instance.GetSquads(OnSquads);
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

    public void OnSquadSelected(SquadItem item)
    {

    }

    private void OnCreatures(UserCreature[] creatures)
    {
        _creatures = creatures;
    }
}
