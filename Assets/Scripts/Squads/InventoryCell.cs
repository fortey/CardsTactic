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
    public int Count
    {
        get => _count;
        set
        {
            _count = value;
            _countLabel.text = _count.ToString();
            if (_count == 0)
                _lastListItem.SetDisable();
        }
    }


    private void Start()
    {
        //_creature = new UserCreature { name = "Mousy", count = 2 };
        //Initialize(_creature);
    }

    public void Initialize(UserCreature creature, Pool creatureItemPool)
    {
        _count = creature.count;

        _countLabel.text = _count.ToString();

        _listItem = creatureItemPool.Get().GetComponent<CreatureListItem>();

        _listItem.Initialize(creature, transform);
        _lastListItem = _listItem;
    }

    public void Pop()
    {

        _lastListItem = Instantiate(_listItem, transform.position, Quaternion.identity, transform);
        Count--;
    }

    public void Push(CreatureListItem item)
    {
        Destroy(_lastListItem.gameObject);
        _lastListItem = item;
        _listItem = item;
        Count++;
    }
}
