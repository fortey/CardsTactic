using System;
using System.Collections;
using System.Collections.Generic;
using Colyseus;
using UnityEngine;
using System.Linq;
using GameDevWare.Serialization;

public class ArenaLobbyController : MonoBehaviour
{
    [SerializeField] private GameRoomController _gameRoomController;
    private ColyseusRoom<dynamic> _lobby;
    private ColyseusRoomAvailable[] _allRooms;
    private ColyseusClient _client;
    [SerializeField] private static NetworkedUser _currentNetworkedUser;

    [SerializeField] private Pool _roomItemsPool;
    public bool Bot { get; set; }

    public async void JoinOrCreateRoom(ColyseusClient client, Dictionary<string, object> options)
    {
        _client = client;
        try
        {
            _lobby = await client.JoinOrCreate("arena_lobby");

            RegisterRoomHandlers();
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
    }

    private void RegisterRoomHandlers()
    {
        _lobby.OnMessage<ColyseusRoomAvailable[]>("rooms", (rooms) =>
        {
            GetAvailableRooms();
        });

        _lobby.OnMessage<object[]>("+", (data) =>
        {
            GetAvailableRooms();
        });

        _lobby.OnMessage<string>("-", (roomId) =>
        {
            GetAvailableRooms();
        });

        // _room.OnMessage<NetworkedUser>("onJoin", currentNetworkedUser =>
        // {
        //     Debug.Log($"Received 'ExampleNetworkedUser' after join/creation call {currentNetworkedUser.id}!");
        //     //Debug.Log(Json.SerializeToString(currentNetworkedUser));

        //     _currentNetworkedUser = currentNetworkedUser;
        // });

        //_room.OnMessage<object>("start", StartGame);

        _lobby.colyseusConnection.OnError += Room_OnError;
        _lobby.colyseusConnection.OnClose += Room_OnClose;
    }

    private static void Room_OnClose(int closeCode)
    {
        Debug.Log("Room_OnClose: " + closeCode);
    }

    private static void Room_OnError(string errorMsg)
    {
        Debug.Log("Room_OnError: " + errorMsg);
    }

    public void CreateGameRoom()
    {
        Dictionary<string, object> roomOptions = new Dictionary<string, object>
        {
            ["name"] = "Вася", //The name of our custom logic file
            ["minReqPlayers"] = 2,
            ["bot"] = Bot
        };
        _gameRoomController.gameObject.SetActive(true);
        _gameRoomController.CreateRoom(_client, roomOptions);
    }

    private void JoinRoom(string roomID)
    {
        Dictionary<string, object> roomOptions = new Dictionary<string, object>
        {
            ["logic"] = "shootingGallery", //The name of our custom logic file
            ["minReqPlayers"] = 1
        };
        _gameRoomController.gameObject.SetActive(true);
        _gameRoomController.JoinRoom(_client, roomID, roomOptions);

    }

    private async void GetAvailableRooms()
    {
        _allRooms = await _client.GetAvailableRooms("game_room");
        RefreshRoomList();
    }

    private void OnApplicationQuit()
    {
        if (_lobby != null) _lobby.Leave();
    }

    private void RefreshRoomList()
    {
        var roomItems = GetComponentsInChildren<RoomListItem>();
        foreach (var roomItem in roomItems)
        {
            _roomItemsPool.Push(roomItem.gameObject);
        }

        foreach (var room in _allRooms)
        {
            var roomItem = _roomItemsPool.Get().GetComponent<RoomListItem>();
            roomItem.Setup(room.name, () =>
            {
                JoinRoom(room.roomId);
                _roomItemsPool.gameObject.SetActive(false);
            });
        }
    }

    private void OnDisable()
    {
        if (_lobby != null) _lobby.Leave();
    }

    private void OnDestroy()
    {
        if (_lobby != null) _lobby.Leave();
    }
}
