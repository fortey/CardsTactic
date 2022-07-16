using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Board : MonoBehaviour
{
    public event System.Action<int> OnCellClick;
    [SerializeField] private Cell[] _cells;

    public Cell this[int index]
    {
        get => _cells[index];
    }

    private void Awake()
    {
        for (int i = 0; i < _cells.Length; i++)
        {
            var index = i;
            _cells[i].GetComponent<Button>().onClick.AddListener(() => OnCellClick?.Invoke(index));
        }
    }

    public void ClearCells()
    {
        foreach (var cell in _cells)
        {
            cell.SetNormal();
        }
    }

    public void SetAvailableCells(int[] cells)
    {
        foreach (var i in cells)
        {
            _cells[i].SetAvailableToMove();
        }
    }
}
