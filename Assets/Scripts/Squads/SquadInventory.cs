using System.Collections.Generic;
using UnityEngine;

public class SquadInventory : MonoBehaviour
{
    [SerializeField] private Pool _cellPool;
    [SerializeField] private UnityEngine.UI.GridLayoutGroup _gridLayout;


    private Dictionary<string, InventoryCell> _cells = new Dictionary<string, InventoryCell>();

    public void Initialize(SquadBoard squadBoard, UserCreature[] userCreatures, Pool creatureItemPool)
    {
        _gridLayout.enabled = true;
        foreach (var userCreature in userCreatures)
        {
            var cell = _cellPool.Get().GetComponent<InventoryCell>();
            cell.Initialize(userCreature, creatureItemPool);

            _cells.Add(userCreature.name, cell);
        }
        Invoke(nameof(DisableGridLayout), 0.3f);

        for (int i = 0; i < squadBoard.Count; i++)
        {
            var creatureItem = squadBoard[i].listItem;
            if (creatureItem)
            {
                var inventoryCell = _cells[creatureItem.Name];
                inventoryCell.Count--;
                creatureItem.inventoryCell = inventoryCell.transform;
                creatureItem.SetBlockRaycasts(true);
            }
        }
    }

    private void DisableGridLayout()
    {
        _gridLayout.enabled = false;
    }

}
