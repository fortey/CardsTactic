using System.Collections;
using System.Collections.Generic;
using Colyseus;
using GameDevWare.Serialization;
using UnityEngine;

public class MyColyseusManager : ColyseusManager<MyColyseusManager>
{
    [SerializeField] private GameRoomController _roomController;
    [SerializeField] private ArenaLobbyController _arenaController;
    private ColyseusRoom<GameRoomState> _room;
    private ColyseusRoom<MainRoomState> _mainRoom;
    [SerializeField] private static NetworkedUser _currentNetworkedUser;

    [SerializeField] private Auth _auth;
    protected override void Start()
    {
        // Dictionary<string, object> roomOptions = new Dictionary<string, object>
        // {
        //     ["logic"] = "",
        //     ["minReqPlayers"] = 1
        // };

        InitializeClient();

        _auth.gameObject.SetActive(true);

        //_arenaController.JoinOrCreateRoom(client, roomOptions);
    }
    public override void InitializeClient()
    {
        base.InitializeClient();
    }

    public async void ConnectToMain(string userName)
    {
        Dictionary<string, object> roomOptions = new Dictionary<string, object>
        {
            ["name"] = userName,

        };

        _mainRoom = await client.JoinOrCreate<MainRoomState>("main_room", roomOptions);

        RegisterRoomHandlers();
    }

    private void RegisterRoomHandlers()
    {
        _mainRoom.OnMessage<NetworkedUser>("onJoin", currentNetworkedUser =>
       {
           Debug.Log($"Received 'ExampleNetworkedUser' after join/creation call {currentNetworkedUser.id}!");
           Debug.Log(Json.SerializeToString(currentNetworkedUser));

           _currentNetworkedUser = currentNetworkedUser;

           _arenaController.JoinOrCreateRoom(client, null);
       });

        _mainRoom.OnMessage<Squad[]>("squads", squads => print(squads[0]));
    }
    private void ClearRoomHandlers()
    {

        if (_room == null)
        {
            return;
        }


    }

    public void GetSquads()
    {
        _mainRoom.Send("getSquads");
    }

}
