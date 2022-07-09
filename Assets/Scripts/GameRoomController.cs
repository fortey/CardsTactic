using System;
using System.Collections;
using System.Collections.Generic;
using Colyseus;
using GameDevWare.Serialization;
using UnityEngine;

public class GameRoomController : MonoBehaviour
{
    public event Action<Creature> OnCreatureSelected;
    private ColyseusRoom<GameRoomState> _room;
    private string _lastRoomId;
    private ColyseusClient _client;

    [SerializeField] private Board _board;
    [SerializeField] private Creature _creaturePrefab;

    [SerializeField] private static NetworkedUser _currentNetworkedUser;

    private Dictionary<string, Creature> _creatures = new Dictionary<string, Creature>();
    //private int _currentCellIndex = -1;
    private int[] _availableToMoveCells;
    private Creature _selectedCreature;
    private string _selectedAction;
    private void Start()
    {
        _board.OnCellClick += CellClickHandler;
    }

    public async void JoinOrCreateRoom(ColyseusClient client, Dictionary<string, object> options)
    {
        _client = client;
        try
        {
            _room = await client.JoinOrCreate<GameRoomState>("game_room", options);

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

        _room.OnMessage<NetworkedUser>("onJoin", currentNetworkedUser =>
        {
            Debug.Log($"Received 'ExampleNetworkedUser' after join/creation call {currentNetworkedUser.id}!");
            Debug.Log(Json.SerializeToString(currentNetworkedUser));

            _currentNetworkedUser = currentNetworkedUser;
        });

        _room.OnMessage<object>("start", StartGame);
        _room.OnMessage<int[]>("available_cells", onAvailableCells);
        _room.OnMessage<int[]>("available_targets", OnAvailableTargets);

        _room.State.creatures.OnAdd += OnCreatureAdd;
        _room.State.board.OnChange += OnBoardChange;

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
        Debug.Log("state changed" + isFirstState);
    }

    private void OnBoardChange(int index, string value)
    {
        Debug.Log($"board {index} - {value}");
        if (value != "")
        {
            var creature = _creatures[value];
            creature.Move(_board[index].transform.position);
            if (creature.Owner == _currentNetworkedUser.sessionId)
            {
                _room.Send("select_cell", index);
            }
        }
    }

    private void OnCreatureAdd(string id, CreatureSchema schema)
    {
        //Debug.Log(schema.id);
        //GameObject.FindObjectOfType<Creature>().Initialize(schema);
    }

    private static void Room_OnClose(int closeCode)
    {
        Debug.Log("Room_OnClose: " + closeCode);
    }

    private static void Room_OnError(string errorMsg)
    {
        Debug.Log("Room_OnError: " + errorMsg);
    }
    #endregion

    #region Server messages
    private void StartGame(object message)
    {
        for (int i = 0; i < _room.State.board.Count; i++)
        {
            if (_room.State.board[i] != "")
            {
                CreateCreature(i);
            }
        }
    }

    private void onAvailableCells(int[] cells)
    {
        _board.ClearCells();
        _board.SetAvailableCells(cells);
        _availableToMoveCells = cells;
    }

    private void OnAvailableTargets(int[] cells)
    {
        foreach (var cell in cells)
        {
            print(_creatures[_room.State.board[cell]].name);
        }
    }
    #endregion

    private void CellClickHandler(int index)
    {
        _board.ClearCells();
        var creatureID = _room.State.board[index];
        if (creatureID != "")
        {
            if (!_creatures.ContainsKey(creatureID)) return;

            var creature = _creatures[creatureID];

            _selectedCreature = creature;
            OnCreatureSelected?.Invoke(creature);

            if (creature.Owner == _currentNetworkedUser.sessionId)
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

    private void CreateCreature(int boardIndex)
    {
        var creatureID = _room.State.board[boardIndex];
        var creatureSchema = _room.State.creatures[creatureID];

        var creature = Instantiate(_creaturePrefab, _board[boardIndex].transform.position, Quaternion.identity, transform);
        creature.Initialize(creatureSchema, creatureSchema.owner != _currentNetworkedUser.sessionId);

        _creatures[creatureID] = creature;
    }

    public void OnAbilityClick(string abilityName)
    {
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
}
