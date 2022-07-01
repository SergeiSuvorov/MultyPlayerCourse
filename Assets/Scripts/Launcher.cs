using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class Launcher : MonoBehaviourPunCallbacks
{

    [SerializeField] Button _connectButton;
    [SerializeField] Button _disconnectButton;

    private void Awake()
    {
        _connectButton.onClick.AddListener(Connect);
        _disconnectButton.onClick.AddListener(Disconnect);

        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public void Connect()
    {

        if (PhotonNetwork.IsConnected)
        {
            Debug.Log("Already connection");
            //PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.GameVersion = Application.version;
        }
    }
    public override void OnConnectedToMaster()
    {
        Debug.Log("Complete OnConnectedToMaster()  ");
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("Complete OnJoinedLobby()  ");
        PhotonNetwork.JoinOrCreateRoom("roomName", new RoomOptions { MaxPlayers = 2, IsVisible = true }, TypedLobby.Default);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Complete OnJoinedRoom() ");
    }

    public override void OnLeftLobby()
    {
        base.OnLeftLobby();
        Debug.Log("Complete OnLeftLobby() ");
        PhotonNetwork.Disconnect();
    }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        Debug.Log("Complete OnLeftRoom() ");
        PhotonNetwork.Disconnect();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        base.OnDisconnected(cause);
        Debug.Log("Complete OnDisconnected() ");
    }
    public void Disconnect()
    {
        if (PhotonNetwork.IsConnected)
            PhotonNetwork.LeaveRoom();
        else
            Debug.Log("Already disconnection");
    }

    private void OnDestroy()
    {
        _connectButton.onClick.RemoveAllListeners();
        _disconnectButton.onClick.RemoveAllListeners();
    }
}
