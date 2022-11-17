using System;
using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class lobbymanager : MonoBehaviourPunCallbacks
{
    
    // Panels
    [SerializeField]
    GameObject MainPanel;
    
    [SerializeField]
    GameObject SoloPanel;
    
    [SerializeField]
    GameObject IAPanel;
    
    [SerializeField]
    GameObject PVPPanel;

    [SerializeField] private TMP_Text UserName;
    
    
    //--------------------------Panel Management------------------
    void Start()
    {
        PhotonNetwork.JoinLobby();
        OnClickBackToMain();
        UserName.text = "Welcome " + PhotonNetwork.NickName;
    }

    public void OnClickSolo()
    {
        MainPanel.SetActive(false);
        SoloPanel.SetActive(true);
        PhotonNetwork.CreateRoom(PhotonNetwork.NickName, new RoomOptions() { IsOpen = false, IsVisible = false });
        
        var hash = PhotonNetwork.LocalPlayer.CustomProperties;
        hash.Add("NumberInRoom",0);
        PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
    }
    public void OnClickIA()
    {
        MainPanel.SetActive(false);
        IAPanel.SetActive(true);
        
        PhotonNetwork.CreateRoom(PhotonNetwork.NickName, new RoomOptions() { IsOpen = false, IsVisible = false });
        
        var hash = PhotonNetwork.LocalPlayer.CustomProperties;
        hash.Add("NumberInRoom",0);
        PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
    }
    public void OnClickPVP()
    {
        MainPanel.SetActive(false);
        PVPPanel.SetActive(true);
        lobbyPanel.SetActive(true);
        roomPanel.SetActive(false);
    }

    public void OnClickBackToMain()
    {
        MainPanel.SetActive(true);
        SoloPanel.SetActive(false);
        IAPanel.SetActive(false);
        PVPPanel.SetActive(false);
    }

    public void OnClickBack()
    {
        PhotonNetwork.Disconnect();
        SceneManager.LoadScene("StartMenu");
    }
    
    //--------------------------Solo Time Attack------------------
    
    public void OnClickMap1()
    {
        if (PhotonNetwork.CurrentRoom != null)
        {
            PhotonNetwork.LoadLevel("Circuit1");
        }
    }

    public void OnClickMap2()
    {
        
        if (PhotonNetwork.CurrentRoom != null)
        {
            PhotonNetwork.LoadLevel("Circuit_Ach");
        }
        
    }


    //--------------------------Solo VS IA------------------------

    public void OnClickMap1IA()
    {
        if (PhotonNetwork.CurrentRoom != null)
        {
            PhotonNetwork.LoadLevel("Circuit1IA");
        }
    }
    
    
    //--------------------------Multiplayer-----------------------

    public TMP_InputField RoomInputField;
    public GameObject lobbyPanel;
    public GameObject roomPanel;
    public TMP_Text roomName;
    public RoomItem RoomItemPrefab;
    private List<RoomItem> roomItemList = new List<RoomItem>();
    public Transform contentObject;
    public float timeBetweenUpdate = 1.5f;
    private float nextUpdateTime;
    public List<PlayerItem> playerItemsList = new List<PlayerItem>();
    public PlayerItem PlayerItemPrefab;
    public Transform playerItemParent;
    public GameObject buttonStart;

    public void OnClickCreate()
    {
        if (RoomInputField.text.Length >= 1)
        {
            PhotonNetwork.CreateRoom(RoomInputField.text, new RoomOptions() { MaxPlayers = 4 });
        }
    }

    public override void OnJoinedRoom()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            buttonStart.SetActive(true);
        }
        lobbyPanel.SetActive(false);
        roomPanel.SetActive(true);
        UserName.text = "";
        roomName.text = "Room Name : " + PhotonNetwork.CurrentRoom.Name;
        UpdatePlayerList();
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        if (Time.time >= nextUpdateTime)
        {
            UpdateRoomList(roomList);
            nextUpdateTime = Time.time + timeBetweenUpdate;
        }
        
    }

    void UpdateRoomList(List<RoomInfo> list)
    {
        foreach (var item in roomItemList)
        {
            Destroy(item.gameObject);
        }
        roomItemList.Clear();

        foreach (RoomInfo room in list)
        {
            RoomItem newRoom = Instantiate(RoomItemPrefab, contentObject);
            newRoom.SetRoomName(room.Name);
            roomItemList.Add(newRoom);
        }
    }

    public void JoinRoom(string roomName)
    {
        PhotonNetwork.JoinRoom(roomName);
    }

    public void OnClickLeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        roomPanel.SetActive(false);
        lobbyPanel.SetActive(true);
        UserName.text = "Welcome " + PhotonNetwork.NickName;
    }

    
    // ReSharper disable Unity.PerformanceAnalysis
    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }

    void UpdatePlayerList()
    {
        foreach (var item in playerItemsList)
        {
            Destroy(item.gameObject);
        }
        playerItemsList.Clear();

        if (PhotonNetwork.CurrentRoom == null)
        {
            return;
        }

        foreach (KeyValuePair<int, Player> player in PhotonNetwork.CurrentRoom.Players)
        {
            PlayerItem newPlayerItem = Instantiate(PlayerItemPrefab, playerItemParent);
            newPlayerItem.SetPlayerInfo(player.Value);
            playerItemsList.Add(newPlayerItem);
        }
    }

    
    
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        UpdatePlayerList();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        UpdatePlayerList();
    }
    
    
    
    public void OnClickStart()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            
            for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
            {

                var hash = PhotonNetwork.PlayerList[i].CustomProperties;
                
                hash.Add("NumberInRoom",i);
                
                //Debug.Log((int)hash["NumberInRoom"]);
                
                PhotonNetwork.PlayerList[i].SetCustomProperties(hash);

                Debug.Log(PhotonNetwork.PlayerList[i].CustomProperties["NumberInRoom"]);
            }
        }
        PhotonNetwork.LoadLevel("Circuit1");
    }
    
}
