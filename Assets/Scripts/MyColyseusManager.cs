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
            ["logic"] = "shootingGallery", //The name of our custom logic file
            ["minReqPlayers"] = 1
        };

        //_roomController = new GameRoomController();
        //_roomController.SetRoomOptions(roomOptions);
        //_roomController.SetDependencies(_colyseusSettings);

        InitializeClient();

        _arenaController.JoinOrCreateRoom(client, roomOptions);
    }
    public override void InitializeClient()
    {
        base.InitializeClient();

        //_roomController.JoinOrCreateRoom(client, roomOptions);
        //EnterRoom();
    }

    private async void EnterRoom()
    {
        Dictionary<string, object> roomOptions = new Dictionary<string, object>
        {
            ["logic"] = "shootingGallery", //The name of our custom logic file
            ["minReqPlayers"] = 1
        };
        _room = await client.JoinOrCreate<GameRoomState>("game_room", roomOptions);

        _room.State.creatures.OnAdd += (id, creature) => print(creature);
    }

}
