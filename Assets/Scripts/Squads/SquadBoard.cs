using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquadBoard : MonoBehaviour
{
    [SerializeField] private Cell[] _cells;
    public Cell this[int index]
    {
        get => _cells[index];
    }
}
