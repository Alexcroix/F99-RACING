using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class RoomItem : MonoBehaviour
{
    public TMP_Text roomName;
    private lobbymanager manager;

    private void Start()
    {
        manager = FindObjectOfType<lobbymanager>();
    }

    public void SetRoomName(string _roomName)
    {
        roomName.text = _roomName;
    }

    public void OnClickItem()
    {
        manager.JoinRoom(roomName.text);
    }
}
