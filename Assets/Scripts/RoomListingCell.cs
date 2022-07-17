using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomListingCell : MonoBehaviour
{
    [SerializeField]
    private Text _text;
    [SerializeField]
    private Button _button;

    public Action<RoomInfo> ChoiceRoom;

    public RoomInfo RoomInfo { get; private set; }

    private void Awake()
    {
        _button.onClick.AddListener(onButtonClick);
    }

    public void SetRoomInfo(RoomInfo roomInfo)
    {
        RoomInfo = roomInfo;

        _text.text = roomInfo.Name + " ,Players " + roomInfo.PlayerCount +"/"+ roomInfo.MaxPlayers;
        ChoiceRoom = null;
    }
    
    private void onButtonClick()
    {
        ChoiceRoom?.Invoke(RoomInfo);
    }

    private void OnDisable()
    { 
        ChoiceRoom = null;
    }

}
