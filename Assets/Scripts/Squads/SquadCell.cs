using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SquadCell : MonoBehaviour, IDropHandler
{
    private CreatureListItem _listItem;
    public void OnDrop(PointerEventData eventData)
    {
        var otherItemTransform = eventData.pointerDrag.transform;
        if (otherItemTransform.TryGetComponent<CreatureListItem>(out CreatureListItem item))
        {
            if (!_listItem)
                SetItem(otherItemTransform, item);
            else if (_listItem && item.previousCell && item.previousCell.TryGetComponent<SquadCell>(out SquadCell squadCell))
            {
                _listItem.previousCell = squadCell.transform;
                squadCell.SetItem(_listItem.transform, _listItem);

                SetItem(otherItemTransform, item);
            }
            else item.ReturnToInventory();
        }
    }

    public void SetItem(Transform itemTransform, CreatureListItem item)
    {
        itemTransform.SetParent(transform);
        itemTransform.localPosition = Vector3.zero;
        _listItem = item;
    }

    public void Clear()
    {
        _listItem = null;
    }
}
