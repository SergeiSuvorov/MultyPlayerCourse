using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using System.Linq;
using ExitGames.Client.Photon;
using UnityEngine.UI;
using System;

public class ConnectAndJoinRoom : MonoBehaviour, IConnectionCallbacks, IMatchmakingCallbacks
{
    [SerializeField]
    private ServerSettings _serverSettings;

    [SerializeField]
    private TMP_Text _stateUiText;

    [SerializeField]
    private TMP_Text _messageText;

    [SerializeField] Button _connectButton;
    [SerializeField] Button _createRoomButton;
    [SerializeField] Button _closeRoomButton;
    [SerializeField] Button _exitFromRoomButton;
    [SerializeField] Text _roomName;

    [SerializeField] RoomListing _roomList;
    [SerializeField] RoomPlayerInfo _roomPlayerInfo;

    private const string GAME_MOD_KEY = "gm";
    private const string AI_MOD_KEY = "ai";

    private const string MONEY_PROP_KEY = "C0";
    private const string MAP_PROP_KEY = "C1";

    private TypedLobby _sqlLobby = new TypedLobby("sqlLobby", LobbyType.SqlLobby);
    private TypedLobby _customLobby = new TypedLobby("customLobby", LobbyType.Default);

    private LoadBalancingClient _lbc;

    private void Start()
    {
        if (enabled)
        {
            _connectButton.onClick.AddListener(ConnectToRoom);
            _createRoomButton.onClick.AddListener(CreateRoom);
            _closeRoomButton.onClick.AddListener(CloseRoom);
            _exitFromRoomButton.onClick.AddListener(ExitRoom);

            _lbc = new LoadBalancingClient();
            _lbc.AddCallbackTarget(this);

            _roomList.ChoiceRoom += JoinInRoom;
            _roomList.SetLoadBalancingClient(_lbc);

            _roomPlayerInfo.SetLoadBalancingClient(_lbc);

            if (!_lbc.ConnectUsingSettings(_serverSettings.AppSettings))
                Debug.LogError("Error connection!");

            _roomPlayerInfo.gameObject.SetActive(false);
            _roomList.gameObject.SetActive(true);
        }
        else
            Debug.Log("NotEnable ConnectAndJoinRoom");
    }

    private void Update()
    {
        if (_lbc == null)
            return;

        _lbc.Service();

        var state = _lbc.State.ToString();
        _stateUiText.text = $"State: {state}, Room:{_lbc.RoomsCount} ,userId: {_lbc.UserId}";
    }

    private void OnDestroy()
    {
        if (_lbc != null)
            _lbc.RemoveCallbackTarget(this);
    }

    private void ConnectToRoom()
    {
        if (_roomName.text == string.Empty)
        {
            Debug.Log("Enter room name");
           
            return;
        }
        _messageText.text = "";
        var enterRoomParams = new EnterRoomParams { RoomName = _roomName.text };
        _lbc.OpJoinRoom(enterRoomParams);
    }

    private void ExitRoom()
    {
        if(_lbc.InRoom)
        {
            Debug.Log("ExitFromRoom");
            _lbc.OpLeaveRoom(false);
        }
    }

    private void CloseRoom()
    {
        if (_lbc.IsConnected && _lbc.InRoom)
        {
            _lbc.CurrentRoom.IsOpen = false;
            Debug.Log("CloseRoom");
        }
    }

   
    private void JoinInRoom(RoomInfo info)
    {
        if (!_lbc.IsConnected || _lbc.InRoom)
            return;
        var enterRoomParams = new EnterRoomParams { RoomName = info.Name};
        _lbc.OpJoinRoom(enterRoomParams);
    }
    private void CreateRoom()
    {
        if (!_lbc.IsConnected)
        {
            Debug.Log("Not Connecting");
            return;
        }
        if (!_lbc.InLobby)
        {
            Debug.Log("Not Connecting");
            return;
        }
        if (_lbc.InRoom)
        {
            Debug.Log("You alredy in room. Exit before create new");
            return;
        }
        string roomName = _lbc.UserId;

        if (_roomName.text != string.Empty)
        {
            roomName = _roomName.text;
        }

        var roomOptions = new RoomOptions
        {
            MaxPlayers = 12,
            IsVisible = true
            ,PublishUserId = true
        };
       
       
        var enterRoomParams = new EnterRoomParams { RoomName = roomName, RoomOptions = roomOptions, Lobby = _customLobby,/* ExpectedUsers = new[] { "232323232", "324234334" } */};
        _lbc.OpCreateRoom(enterRoomParams);
    }
  

    public void OnConnectedToMaster()
    {
        Debug.Log("OnConnectedToMaster");
        _lbc.NickName = _lbc.UserId;
        _lbc.OpJoinLobby(_customLobby);
    }

    public void OnJoinedRoom()
    {
        Debug.Log("OnJoinedRoom " + _lbc.CurrentRoom.Name);
        Debug.Log("OnJoinedRoom " + _lbc.CurrentRoom.PlayerCount);
        _roomPlayerInfo.gameObject.SetActive(true);
        _roomList.gameObject.SetActive(false);
        _messageText.text = "";
    }

    public void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("OnJoinRandomFailed");
        _lbc.OpCreateRoom(new EnterRoomParams());
    }

    public void OnJoinRoomFailed(short returnCode, string message)
    {
        _lbc.OpJoinLobby(_customLobby);
    }

    public void OnLeftRoom()
    {
        _roomPlayerInfo.gameObject.SetActive(false);
        _roomList.gameObject.SetActive(true);
    }

    public void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log(returnCode + " OnCreateRoomFailed " + message);
        if(returnCode == 32766)
        {
            _messageText.color = Color.red;
            _messageText.text = "Room whith this name already exist";
        }
        _lbc.OpJoinLobby(_customLobby);
    }


    public void OnConnected()
    {
    }
    public void OnCreatedRoom()
    {
    }
    public void OnCustomAuthenticationFailed(string debugMessage)
    {
    }
    public void OnCustomAuthenticationResponse(Dictionary<string, object> data)
    {
    }
    public void OnDisconnected(DisconnectCause cause)
    {
    }
    public void OnFriendListUpdate(List<FriendInfo> friendList)
    {
    }
    public void OnRegionListReceived(RegionHandler regionHandler)
    {
    }
}

