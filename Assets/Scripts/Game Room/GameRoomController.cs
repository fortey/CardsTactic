using System;
using System.Collections;
using System.Collections.Generic;
using Colyseus;
using GameDevWare.Serialization;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class GameRoomController : MonoBehaviour
{
    public event Action<Creature> OnCreatureSelected;
    public event Action<Creature> OnSelectedCreatureChanged;
    public event Action<bool> OnTurnChanged;
    private ColyseusRoom<GameRoomState> _room;
    private string _lastRoomId;
    private ColyseusClient _client;

    [SerializeField] private Board _board;
    [SerializeField] private Creature _creaturePrefab;
    [SerializeField] private Transform _creaturesParent;
    [SerializeField] private GameObject _WaitingOpponent;

    [SerializeField] private static NetworkedUser _currentNetworkedUser;

    private Dictionary<string, Creature> _creatures = new Dictionary<string, Creature>();
    //private int _currentCellIndex = -1;
    private int[] _availableToMoveCells;
    private Creature _selectedCreature;
    private string _selectedAction = "";
    private int[] _availableTargetCells;
    private bool _myTurn;

    private bool _isStarted;
    private void Start()
    {
        _board.OnCellClick += CellClickHandler;
    }

    public async void CreateRoom(ColyseusClient client, Dictionary<string, object> options)
    {
        _client = client;
        try
        {
            _room = await client.JoinOrCreate<GameRoomState>("game_room", options);

            _lastRoomId = _room.Id;
            RegisterRoomHandlers();

            _WaitingOpponent.SetActive(true);
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
    }

    public async void JoinRoom(ColyseusClient client, string roomID, Dictionary<string, object> options)
    {
        _client = client;
        try
        {
            _room = await client.JoinById<GameRoomState>(roomID, options);

            _lastRoomId = _room.Id;
            RegisterRoomHandlers();
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
    }

    private void RegisterRoomHandlers()
    {
        _room.OnStateChange += OnStateChangeHandler;
        _room.State.OnChange += OnStateChange;

        _room.OnMessage<NetworkedUser>("onJoin", currentNetworkedUser =>
        {
            Debug.Log($"Received 'ExampleNetworkedUser' after join/creation call {currentNetworkedUser.id}!");
            Debug.Log(Json.SerializeToString(currentNetworkedUser));

            _currentNetworkedUser = currentNetworkedUser;
            ChangeTurn();
        });

        _room.OnMessage<object>("start", StartGame);
        _room.OnMessage<int[]>("available_cells", onAvailableCells);
        _room.OnMessage<int[]>("available_targets", OnAvailableTargets);
        _room.OnMessage<int[]>("action", OnAction);

        _room.State.creatures.OnAdd += OnCreatureAdd;
        _room.State.board.OnChange += OnBoardChange;
        _room.State.graveyard.OnAdd += OnGraveyardAdd;

        _room.colyseusConnection.OnError += Room_OnError;
        _room.colyseusConnection.OnClose += Room_OnClose;
    }

    private void ClearRoomHandlers()
    {

        if (_room == null)
        {
            return;
        }

        _room.State.creatures.OnAdd -= OnCreatureAdd;
        //_room.State.creatures.OnRemove -= OnEntityRemoved;

        _room.colyseusConnection.OnError -= Room_OnError;
        _room.colyseusConnection.OnClose -= Room_OnClose;

        _room.OnStateChange -= OnStateChangeHandler;
        _room.State.OnChange -= OnStateChange;
        _room.State.board.OnChange -= OnBoardChange;

        //_room.OnLeave -= OnLeaveRoom;

        _room = null;
        _currentNetworkedUser = null;
    }

    #region State events
    private static void OnStateChangeHandler(GameRoomState state, bool isFirstState)
    {
        // Setup room first state
        //LSLog.LogImportant("State has been updated!");
        //Debug.Log("state changed" + isFirstState);

    }

    private void OnStateChange(List<Colyseus.Schema.DataChange> changes)
    {
        foreach (var change in changes)
        {
            if (change.Field == "currentTurn" && _currentNetworkedUser != null)
            {
                ChangeTurn();
            }
        }
    }

    private void OnBoardChange(int index, string value)
    {
        if (value != "" && _isStarted)
        {
            var creature = _creatures[value];
            creature.Move(_board[index].transform.position);
            if (creature.Owner == _currentNetworkedUser.sessionId)
            {
                _room.Send("select_cell", index);
            }
            UpdateSelectedCell();
        }
    }

    private void OnCreatureAdd(string id, CreatureSchema schema)
    {
        if (_currentNetworkedUser == null) return;
        //Debug.Log(schema.id);
        //GameObject.FindObjectOfType<Creature>().Initialize(schema);

        var creature = Instantiate(_creaturePrefab, transform.position, Quaternion.identity, _creaturesParent);
        creature.Initialize(schema, schema.owner != _currentNetworkedUser.sessionId);

        _creatures[id] = creature;

        creature.OnChange += (creature) =>
        {
            if (_selectedCreature == creature) OnSelectedCreatureChanged?.Invoke(creature);
        };

        //schema.OnChange += creature.OnStateChanged;
    }

    private void OnGraveyardAdd(int index, string value)
    {
        if (value != "")
        {
            var creature = _creatures[value];
            creature.gameObject.SetActive(false);
        }
    }

    private void Room_OnClose(int closeCode)
    {
        Debug.Log("Room_OnClose: " + closeCode);
        _room = null;
    }

    private void Room_OnError(string errorMsg)
    {
        Debug.Log("Room_OnError: " + errorMsg);
    }

    private void OnCurrentTurnChanged() { }

    #endregion

    #region Server messages
    private void StartGame(object message)
    {
        _isStarted = true;
        for (int i = 0; i < _room.State.board.Count; i++)
        {
            print(_room.State.board[i]);
            if (_room.State.board[i] != "")
            {
                CreateCreature(i);
            }
        }

        _WaitingOpponent.SetActive(false);
        Global.Instance.Squads.SelectSquad(OnSquadSelected);
    }

    private void onAvailableCells(int[] cells)
    {
        _board.ClearCells();
        _board.SetAvailableCells(cells);
        UpdateSelectedCell();
        _availableToMoveCells = cells;
    }

    private void OnAvailableTargets(int[] cells)
    {
        _availableTargetCells = cells;
        foreach (var cell in cells)
        {
            var creature = _creatures[_room.State.board[cell]];
            if (creature)
                creature.SetTarget(true);
        }
    }

    private void OnAction(int[] cells)
    {
        var arrow = Global.Instance.ArrowPool.Get();
        arrow.GetComponent<Arrow>().Show(_board[cells[0]].transform.position, _board[cells[1]].transform.position);
    }

    #endregion

    private void CellClickHandler(int index)
    {
        if (_selectedAction == "")
        {
            _board.ClearCells();
            ClearTargetCreatures();

            var creatureID = _room.State.board[index];
            if (creatureID != "")
            {
                if (!_creatures.ContainsKey(creatureID)) return;

                var creature = _creatures[creatureID];

                _selectedCreature = creature;
                _board[index].SetSelected();
                OnCreatureSelected?.Invoke(creature);

                if (creature.Owner == _currentNetworkedUser.sessionId && _myTurn && creature.Active)
                {
                    _room.Send("select_cell", index);
                    //_currentCellIndex = index;
                }
            }
            else
            {
                var isMoving = false;
                if (_selectedCreature && _selectedCreature.Owner == _currentNetworkedUser.sessionId && _availableToMoveCells != null)
                {
                    var currentCellIndex = GetCellIndex(_selectedCreature.ID);
                    if (currentCellIndex != -1)
                    {
                        foreach (var i in _availableToMoveCells)
                        {
                            if (i == index)
                            {
                                _room.Send("move", new int[] { currentCellIndex, index });
                                isMoving = true;
                                break;
                            }
                        }
                    }
                }

                if (!isMoving)
                {
                    _selectedCreature = null;
                    OnCreatureSelected?.Invoke(null);
                }

            }
        }
        else
        {
            if (_availableTargetCells != null && _availableTargetCells.Contains(index))
            {
                var currentCellIndex = GetCellIndex(_selectedCreature.ID);
                if (currentCellIndex != -1)
                {
                    _room.Send("action", new object[] { currentCellIndex, _selectedAction, index });
                }
            }
            _availableTargetCells = null;
            _selectedAction = "";
            ClearTargetCreatures();
        }
    }

    private void CreateCreature(int boardIndex)
    {
        var creatureID = _room.State.board[boardIndex];
        var creatureSchema = _room.State.creatures[creatureID];

        var creature = Instantiate(_creaturePrefab, _board[boardIndex].transform.position, Quaternion.identity, _creaturesParent);
        creature.Initialize(creatureSchema, creatureSchema.owner != _currentNetworkedUser.sessionId);

        _creatures[creatureID] = creature;

        //creatureSchema.OnChange += creature.OnStateChanged;
        creature.OnChange += (creature) =>
        {
            if (_selectedCreature == creature) OnSelectedCreatureChanged?.Invoke(creature);
        };
    }

    public void OnAbilityClick(string abilityName)
    {
        if (abilityName != "pass" && abilityName != "defense" && abilityName != "take_points")
            _selectedAction = abilityName;
        _room.Send("ability_clicked", new object[] { GetCellIndex(_selectedCreature.ID), abilityName });
    }

    private int GetCellIndex(string creatureID)
    {
        for (int i = 0; i < _room.State.board.Count; i++)
        {
            if (_room.State.board[i] == creatureID)
            {
                return i;
            }
        }
        return -1;
    }

    private void ClearTargetCreatures()
    {
        foreach (var creature in _creatures)
        {
            creature.Value.SetTarget(false);
        }
    }

    private void UpdateSelectedCell()
    {
        if (_selectedCreature)
            _board[GetCellIndex(_selectedCreature.ID)].SetSelected();
    }

    public void Pass()
    {
        _room.Send("pass");
    }

    private void OnDisable()
    {
        ClearRoomHandlers();
    }

    private void ChangeTurn()
    {
        _myTurn = _room.State.currentTurn == _currentNetworkedUser.sessionId;
        OnTurnChanged?.Invoke(!_myTurn);
        //_passButton.interactable = _myTurn;
        _selectedAction = "";

        ClearTargetCreatures();
        _board.ClearCells();
        UpdateSelectedCell();
        if (_myTurn)
        {
            if (_selectedCreature && !_selectedCreature.IsEnemy)
            {
                int index = GetCellIndex(_selectedCreature.ID);
                if (index > -1)
                    _room.Send("select_cell", index);
            }
        }
    }

    private void OnApplicationQuit()
    {
        if (_room != null) _room.Leave();
        ClearRoomHandlers();
    }

    // private void OnCreatureChangedHandler(Creature creature)
    // {
    //     if (_selectedCreature == creature)
    //     {
    //         OnSelectedCreatureChanged?.Invoke(creature);
    //     }
    // }

    private void OnSquadSelected(Squad squad)
    {
        _room.Send("squad_selected", squad);
    }
}
