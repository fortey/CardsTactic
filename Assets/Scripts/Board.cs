using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Board : MonoBehaviour
{
    public event System.Action<int> onCellClick;
    public Cell[] cells;

    public Cell this[int index]
    {
        get => cells[index];
    }

    private void Awake()
    {
        for (int i = 0; i < cells.Length; i++)
        {
            var index = i;
            cells[i].GetComponent<Button>().onClick.AddListener(() => onCellClick?.Invoke(index));
        }
    }
}
