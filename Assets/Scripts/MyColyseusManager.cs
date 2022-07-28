using System.Collections;
using System.Collections.Generic;
using Colyseus;
using UnityEngine;

public class MyColyseusManager : ColyseusManager<MyColyseusManager>
{
    [SerializeField] private GameRoomController _roomController;
    [SerializeField] private ArenaLobbyController _arenaController;
    private ColyseusRoom<GameRoomState> _room;
    protected override void Start()
    {
        Dictionary<string, object> roomOptions = new Dictionary<string, object>
        {
            ["logic"] = "",
            ["minReqPlayers"] = 1
        };

        InitializeClient();

        _arenaController.JoinOrCreateRoom(client, roomOptions);
    }
    public override void InitializeClient()
    {
        base.InitializeClient();
    }

}
