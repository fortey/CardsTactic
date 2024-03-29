using System;
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
    [SerializeField] private GameObject _loading;


    private Action<Squad[]> _onSquads;
    private Action<UserCreature[]> _onCreatures;
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
        _loading.SetActive(true);

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

           _loading.SetActive(false);
       });

        _mainRoom.OnMessage<Squad[]>("squads", squads => _onSquads?.Invoke(squads));
        _mainRoom.OnMessage<UserCreature[]>("creatures", creatures => _onCreatures?.Invoke(creatures));
    }
    private void ClearRoomHandlers()
    {

        if (_room == null)
        {
            return;
        }


    }

    public void GetSquads(Action<Squad[]> callback)
    {
        _onSquads = callback;
        _mainRoom.Send("getSquads");
    }

    // public void OnSquads(Squad[] squads)
    // {
    //     _onSquads?.Invoke(squads);
    // }

    public void GetCreatures(Action<UserCreature[]> callback)
    {
        _onCreatures = callback;
        _mainRoom.Send("getCreatures");
    }

    public void SaveSquad(Squad squad)
    {
        _mainRoom.Send("updateSquad", squad);
    }

    public void CreateSquad(Squad squad)
    {
        _mainRoom.Send("createSquad", squad);
    }
}
