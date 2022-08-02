using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquadBoard : MonoBehaviour
{
    [SerializeField] private SquadCell[] _cells;
    public SquadCell this[int index]
    {
        get => _cells[index];
    }

    public int Count { get => _cells.Length; }

}
