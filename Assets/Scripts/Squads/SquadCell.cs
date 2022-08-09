using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SquadCell : MonoBehaviour, IDropHandler
{
    public CreatureListItem listItem;
    public void OnDrop(PointerEventData eventData)
    {
        return;
        // var otherItemTransform = eventData.pointerDrag.transform;
        // if (otherItemTransform.TryGetComponent<CreatureListItem>(out CreatureListItem item))
        // {

        // }
    }

    public void SetItem(CreatureListItem item)
    {
        if (!listItem)
        {
            if (item.previousCell && item.previousCell.TryGetComponent<SquadCell>(out SquadCell squadCell))
                squadCell.listItem = null;
        }
        else if (listItem && item.previousCell && item.previousCell.TryGetComponent<SquadCell>(out SquadCell squadCell))
        {
            listItem.PutToCell(squadCell, true);
            squadCell.listItem = listItem;
        }
        //itemTransform.SetParent(transform);
        //itemTransform.localPosition = Vector3.zero;
        listItem = item;
        //item.previousCell = transform;
        //item.PutToCell(this);
    }

    public void Clear()
    {
        listItem = null;
    }
}
