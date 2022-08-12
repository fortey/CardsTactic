using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Squads : MonoBehaviour
{
    [SerializeField] private Pool _squadItemsPool;
    private List<SquadItem> _squadItems = new List<SquadItem>();

    private UserCreature[] _creatures;
    [SerializeField] private Pool _creatureItemPool;
    [SerializeField] private CreatureListItem _creatureListItemPrefab;
    [SerializeField] private SquadBoard _squadBoard;

    [SerializeField] private SquadInventory _creatureInventory;
    [SerializeField] private GameObject _editMode;
    [SerializeField] private GameObject _squadListMode;
    [SerializeField] private TMPro.TMP_InputField _nameInput;
    [SerializeField] private GameObject _editButton;
    [SerializeField] private GameObject _newButton;
    [SerializeField] private GameObject _selectButton;
    [SerializeField] private GameObject _closeButton;
    private Squad[] _squads;
    private Squad _selectedSquad;

    private bool _squadSelectMode;
    private System.Action<Squad> _onSquadSelectedForGame;

    private void Show()
    {
        gameObject.SetActive(true);
        MyColyseusManager.Instance.GetSquads(OnSquads);
        MyColyseusManager.Instance.GetCreatures(OnCreatures);

        _editButton.SetActive(!_squadSelectMode);
        _newButton.SetActive(!_squadSelectMode);
        _selectButton.SetActive(_squadSelectMode);
        _closeButton.SetActive(!_squadSelectMode);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

    public void SelectSquad(System.Action<Squad> callback)
    {
        _squadSelectMode = true;
        Show();
        _onSquadSelectedForGame = callback;
    }

    public void SquadView()
    {
        _squadSelectMode = false;
        Show();
    }

    private void OnSquads(Squad[] squads)
    {
        if (_selectedSquad != null)
        {
            _selectedSquad = squads.FirstOrDefault(s => s.name == _selectedSquad.name);
        }

        _squads = squads;
        Refresh(squads);
    }

    private void ClearItems()
    {
        foreach (var item in _squadItems)
        {
            _squadItemsPool.Push(item.gameObject);
        }
        _squadItems.Clear();
    }

    private void Refresh(Squad[] squads)
    {
        ClearItems();
        foreach (var squad in squads)
        {
            var squadItem = _squadItemsPool.Get().GetComponent<SquadItem>();
            squadItem.Initialize(squad);
            _squadItems.Add(squadItem);
        }
    }

    public void OnSquadSelected(SquadItem squadItem)
    {
        if (_selectedSquad == squadItem.Squard) return;

        ClearSquadBoard();

        _selectedSquad = squadItem.Squard;

        for (int i = 0; i < squadItem.Squard.board.Length; i++)
        {
            var cell = _squadBoard[i];
            var creatureName = squadItem.Squard.board[i];
            if (creatureName == "") { }
            else
            {
                var creatureItem = _creatureItemPool.Get().GetComponent<CreatureListItem>();

                //cell.SetItem(creatureItem.transform, creatureItem);
                creatureItem.Initialize(creatureName);
                creatureItem.PutToCell(cell);
                //creatureItem.previousCell = cell.transform;
                creatureItem.SetBlockRaycasts(false);
            }
        }
    }

    private void OnCreatures(UserCreature[] creatures)
    {
        _creatures = creatures;
    }

    public void EditSquad()
    {
        if (_selectedSquad != null)
        {
            OpenCloseEditPanel(true);
            _nameInput.text = _selectedSquad.name;
            _creatureInventory.Initialize(_squadBoard, _creatures, _creatureItemPool);
        }
    }

    public void NewSquad()
    {
        ClearSquadBoard();

        _selectedSquad = null;

        OpenCloseEditPanel(true);
        _nameInput.text = "";
        _creatureInventory.Initialize(_squadBoard, _creatures, _creatureItemPool);
    }

    public void SaveSquad()
    {
        if (_nameInput.text == string.Empty) return;

        if (_selectedSquad != null)
        {
            UpdateSquad(_selectedSquad);
            MyColyseusManager.Instance.SaveSquad(_selectedSquad);
        }

        if (_selectedSquad == null)
        {
            _selectedSquad = new Squad();
            _selectedSquad.board = new string[8];

            UpdateSquad(_selectedSquad);
            MyColyseusManager.Instance.CreateSquad(_selectedSquad);
        }

        OpenCloseEditPanel(false);
    }

    public void CancelEdit()
    {
        OpenCloseEditPanel(false);
        Refresh(_squads);
        ClearSquadBoard();
        _selectedSquad = null;
    }

    public void OpenCloseEditPanel(bool open)
    {
        _squadListMode.SetActive(!open);
        _editMode.SetActive(open);
        _creatureInventory.gameObject.SetActive(open);
    }

    private void UpdateSquad(Squad squad)
    {
        squad.name = _nameInput.text;

        for (int i = 0; i < _squadBoard.Count; i++)
        {
            if (_squadBoard[i].listItem)
            {
                squad.board[i] = _squadBoard[i].listItem.Name;
                print($"{i} - {_squadBoard[i].listItem.Name}");
            }
            else
                squad.board[i] = "";
        }
    }
    private void ClearSquadBoard()
    {
        for (int i = 0; i < _squadBoard.Count; i++)
        {
            var creatureItem = _squadBoard[i].listItem;
            if (creatureItem)
            {
                _squadBoard[i].Clear();
                _creatureItemPool.Push(creatureItem.gameObject);
                print($"{i} {creatureItem.gameObject.activeSelf}");
            }
        }
    }

    public void SelectSquadForGame()
    {
        if (_selectedSquad != null)
        {
            _onSquadSelectedForGame(_selectedSquad);
            Hide();
        }
    }
}
