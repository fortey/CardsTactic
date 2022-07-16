using System;
using System.Collections;
using System.Collections.Generic;
using Colyseus;
using UnityEngine;

public class ArenaRoomController : MonoBehaviour
{
    [SerializeField] private GameRoomController _gameRoomController;
    private ColyseusRoom<ArenaRoomState> _room;
    private ColyseusClient _client;
    [SerializeField] private static NetworkedUser _currentNetworkedUser;
    public async void JoinOrCreateRoom(ColyseusClient client, Dictionary<string, object> options)
    {
        _client = client;
        try
        {
            _room = await client.JoinOrCreate<ArenaRoomState>("arena_room", options);

            RegisterRoomHandlers();
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
    }

    private void RegisterRoomHandlers()
    {
        //_room.State.OnChange += OnStateChange;

        _room.OnMessage<NetworkedUser>("onJoin", currentNetworkedUser =>
        {
            Debug.Log($"Received 'ExampleNetworkedUser' after join/creation call {currentNetworkedUser.id}!");
            //Debug.Log(Json.SerializeToString(currentNetworkedUser));

            _currentNetworkedUser = currentNetworkedUser;
        });

        //_room.OnMessage<object>("start", StartGame);

        _room.colyseusConnection.OnError += Room_OnError;
        _room.colyseusConnection.OnClose += Room_OnClose;
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
            ["logic"] = "shootingGallery", //The name of our custom logic file
            ["minReqPlayers"] = 1
        };
        _gameRoomController.gameObject.SetActive(true);
        _gameRoomController.JoinOrCreateRoom(_client, roomOptions);
    }
}
