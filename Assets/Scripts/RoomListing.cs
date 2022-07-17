using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
using System;

public class RoomListing : MonoBehaviour, ILobbyCallbacks
{

    [SerializeField] Transform _content;
    [SerializeField] RoomListingCell _roomListing;

    public Action<RoomInfo> ChoiceRoom;
    private List<RoomListingCell> _listings = new List<RoomListingCell>();
    private LoadBalancingClient _lbc;
    private bool _reloading = false;
    private TypedLobby _customLobby = new TypedLobby("customLobby", LobbyType.Default);
    private int updateCount = 0;

    public void Awake()
    {
        for (int i = 0; i < 10; i++)
        {
            CreateButton();
        }
    }
    public void Update()
    {
        if(_lbc!=null &&
            updateCount != _lbc.RoomsCount)
        {
            updateCount = _lbc.RoomsCount;
            if (_lbc.InLobby)
            {
                Debug.Log("ReloadingLobby ");
                _reloading = true;
                _lbc.OpLeaveLobby();
            }
        }
    }

    public void SetLoadBalancingClient (LoadBalancingClient loadBalancingClient)
    {
        _lbc = loadBalancingClient;
        _lbc.AddCallbackTarget(this);
    }

    public  void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        Debug.Log("OnRoomListUpdate " + roomList.Count);
        Debug.Log("OnRoom " + _lbc.RoomsCount);

        while (roomList.Count> _listings.Count)
        {
            CreateButton();

        }
        int indexList = 0;

       for(int i=0;i<roomList.Count; i++)
        {
            _listings[indexList].gameObject.SetActive(true);
            _listings[indexList].SetRoomInfo(roomList[i]);
            _listings[indexList].ChoiceRoom = onButtonClick;

            if (roomList[i].RemovedFromList && !roomList[i].IsOpen )
            {
                _listings[indexList].gameObject.SetActive(false);
                Debug.Log("RoomName " + roomList[i].Name);
            }

            indexList++;
        }

        Debug.Log("IndexList " + indexList);
 
       for(int i= indexList; i<_listings.Count;i++)
        {
            _listings[i].gameObject.SetActive(false);
        }

    }

    private void ListCountCheck(List<RoomInfo> roomList)
    {
        while (roomList.Count < _listings.Count)
        {

            foreach (var info in _listings)
            {
                int index = roomList.FindIndex(x => x.Name == info.RoomInfo.Name);
                if (index == -1)
                {
                    Destroy(info.gameObject);
                    _listings.Remove(info);
                    break;
                }
            }
        }
    }

    private void onButtonClick(RoomInfo info)
    {
        ChoiceRoom?.Invoke(info);
    }


    private void OnDestroy()
    {
        ChoiceRoom = null;
        if (_lbc != null)
            _lbc.RemoveCallbackTarget(this);
    }

    private void CreateButton()
    {
        RoomListingCell roomListing = Instantiate(_roomListing, _content);
        roomListing.ChoiceRoom = onButtonClick;
        _listings.Add(roomListing);
        roomListing.gameObject.SetActive(false);
    }

    public void OnJoinedLobby()
    {
        Debug.Log("OnJoinedLobby");
    }

    public void OnLeftLobby()
    {
        if (_reloading)
        {
            _reloading = false;
            _lbc.OpJoinLobby(_customLobby);
        }
    }

    public void OnLobbyStatisticsUpdate(List<TypedLobbyInfo> lobbyStatistics)
    {

    }
}

