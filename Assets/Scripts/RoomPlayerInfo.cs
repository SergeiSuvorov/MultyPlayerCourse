using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class RoomPlayerInfo : MonoBehaviour, IInRoomCallbacks
{
    [SerializeField]
    private  TMP_Text _roomInfoText;


    private LoadBalancingClient _lbc;

   
    public void SetLoadBalancingClient(LoadBalancingClient loadBalancingClient)
    {
        _lbc = loadBalancingClient;
        _lbc.AddCallbackTarget(this);
    }

    private void OnEnable()
    {
        if(_lbc!=null && _lbc.InRoom )
        SetPlayerInfo(_lbc.CurrentRoom.Players);
    }
    public void SetPlayerInfo(Dictionary<int,Player> players)
    {
        var sortingPlayers = players.OrderBy(x => x.Key);
    

        string playerList = "Player: "+ players.Count + "\n";

        foreach (var player in sortingPlayers)
        {
            playerList += player.Key + " " + player.Value.UserId + "\n";
        }

        _roomInfoText.text = playerList;

    }

    public void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("OnPlayerEnteredRoom");
        SetPlayerInfo(_lbc.CurrentRoom.Players);
    }

    public void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log("OnPlayerLeftRoom");
        SetPlayerInfo(_lbc.CurrentRoom.Players);
    }

    private void OnDisable()
    {
        if (_lbc != null)
            _lbc.RemoveCallbackTarget(this);
    }

    public void OnMasterClientSwitched(Player newMasterClient)
    {

    }

    public void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
    {

    }

    public void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {

    }
}
