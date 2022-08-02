using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SquadCell : MonoBehaviour, IDropHandler
{
    public CreatureListItem listItem { get; private set; }
    public void OnDrop(PointerEventData eventData)
    {
        var otherItemTransform = eventData.pointerDrag.transform;
        if (otherItemTransform.TryGetComponent<CreatureListItem>(out CreatureListItem item))
        {
            if (!listItem)
            {
                if (item.previousCell && item.previousCell.TryGetComponent<SquadCell>(out SquadCell squadCell))
                    squadCell.listItem = null;
                SetItem(otherItemTransform, item);
            }
            else if (listItem && item.previousCell && item.previousCell.TryGetComponent<SquadCell>(out SquadCell squadCell))
            {
                listItem.previousCell = squadCell.transform;
                squadCell.SetItem(listItem.transform, listItem);

                SetItem(otherItemTransform, item);
            }
            //else item.ReturnToInventory();
        }
    }

    public void SetItem(Transform itemTransform, CreatureListItem item)
    {
        itemTransform.SetParent(transform);
        itemTransform.localPosition = Vector3.zero;
        listItem = item;
        item.previousCell = transform;
    }

    public void Clear()
    {
        listItem = null;
    }
}
