using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryCell : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI _countLabel;
    [SerializeField] private CreatureListItem _listItem;
    private UserCreature _creature;
    private int _count;
    private CreatureListItem _lastListItem;
    private int Count
    {
        get => _count;
        set
        {
            _count = value;
            _countLabel.text = _count.ToString();
        }
    }


    private void Start()
    {
        _creature = new UserCreature { name = "Mousy", count = 2 };
        Initialize(_creature);
    }

    public void Initialize(UserCreature creature)
    {
        _count = creature.count;

        _countLabel.text = _count.ToString();

        _listItem.Initialize(creature);
    }

    public void Pop()
    {
        Count--;
        _lastListItem = Instantiate(_listItem, transform.position, Quaternion.identity, transform);
        if (_count == 0)
            _lastListItem.SetDisable();
    }

    public void Push(CreatureListItem item)
    {
        Destroy(_lastListItem.gameObject);
        _lastListItem = item;
        _listItem = item;
        Count++;
    }
}
