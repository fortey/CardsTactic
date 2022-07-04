using System;
using System.Collections;
using System.Collections.Generic;
using Colyseus;
using GameDevWare.Serialization;
using UnityEngine;

public class GameRoomController : MonoBehaviour
{
    private ColyseusRoom<GameRoomState> _room;
    private string _lastRoomId;
    private ColyseusClient _client;

    [SerializeField] private Board _board;
    [SerializeField] private Creature _creaturePrefab;

    [SerializeField] private static NetworkedUser _currentNetworkedUser;

    private Dictionary<string, Creature> _creatures = new Dictionary<string, Creature>();

    private void Start()
    {
        _board.onCellClick += CellClickHandler;
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

    private static void OnStateChangeHandler(GameRoomState state, bool isFirstState)
    {
        // Setup room first state
        //LSLog.LogImportant("State has been updated!");
        Debug.Log("state changed" + isFirstState);
    }

    private void OnBoardChange(int index, string value)
    {
        Debug.Log($"board {index} - {value}");
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

    private void StartGame(object message)
    {
        for (int i = 0; i < _room.State.board.Count; i++)
        {
            if (_room.State.board[i] != "")
            {
                var creatureID = _room.State.board[i];
                var creatureSchema = _room.State.creatures[creatureID];

                var creature = Instantiate(_creaturePrefab, _board[i].transform.position, Quaternion.identity, transform);
                creature.Initialize(creatureSchema);

                _creatures[creatureID] = creature;
            }
        }
    }

    private void CellClickHandler(int index)
    {
        var creatureID = _room.State.board[index];
        if (creatureID != "")
        {
            _room.Send("select_cell", index);
        }

    }
}
