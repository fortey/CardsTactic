using UnityEngine;
using UnityEngine.UI;

public class Cell : MonoBehaviour
{
    [SerializeField] private Color _normalColor;
    [SerializeField] private Color _availableToMoveColor;
    [SerializeField] private Color _selectedColor;
    [SerializeField] private Image _image;

    public CellState State = CellState.normal;
    public void SetNormal()
    {
        _image.color = _normalColor;
        State = CellState.normal;
    }

    public void SetAvailableToMove()
    {
        _image.color = _availableToMoveColor;
        State = CellState.availableToMove;
    }

    public void SetSelected()
    {
        _image.color = _selectedColor;
        State = CellState.selected;
    }
}

public enum CellState { normal, availableToMove, selected, busy }
