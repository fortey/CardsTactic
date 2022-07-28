using System.Collections;
using System.Collections.Generic;
using Colyseus;
using UnityEngine;

public class MyColyseusManager : ColyseusManager<MyColyseusManager>
{
    [SerializeField] private GameRoomController _roomController;
    [SerializeField] private ArenaLobbyController _arenaController;
    private ColyseusRoom<GameRoomState> _room;
    private ColyseusRoom<MainRoomState> _mainRoom;
    protected override void Start()
    {
        Dictionary<string, object> roomOptions = new Dictionary<string, object>
        {
            ["logic"] = "",
            ["minReqPlayers"] = 1
        };

        InitializeClient();

        ConnectToMain();

        //_arenaController.JoinOrCreateRoom(client, roomOptions);
    }
    public override void InitializeClient()
    {
        base.InitializeClient();
    }

    private async void ConnectToMain()
    {
        Dictionary<string, object> roomOptions = new Dictionary<string, object>
        {
            ["name"] = "player1",

        };

        _mainRoom = await client.JoinOrCreate<MainRoomState>("main_room", roomOptions);
    }
}
