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
        UpdateImageMap();
    }
    public void OnClickIA()
    {
        MainPanel.SetActive(false);
        IAPanel.SetActive(true);
        
        PhotonNetwork.CreateRoom(PhotonNetwork.NickName, new RoomOptions() { IsOpen = false, IsVisible = false });
        
        var hash = PhotonNetwork.LocalPlayer.CustomProperties;
        hash.Add("NumberInRoom",0);
        PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
        UpdateImageMapIA();
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
        if (PhotonNetwork.CurrentRoom != null)
        {
            PhotonNetwork.LeaveRoom();
        }
    }

    public void OnClickBack()
    {
        PhotonNetwork.Disconnect();
        SceneManager.LoadScene("StartMenu");
    }
    
    //--------------------------Solo Time Attack------------------


    public RawImage mapImage;
    public Texture[] ListMapImage;
    public string[] ListMap;
    
    public int mapSelection = 0;
    
    public void OnClickMap1()
    {
        mapSelection = 0;
        UpdateImageMap();
    }

    public void OnClickMap2()
    {
        mapSelection = 1;
        UpdateImageMap();
    }
    
    public void OnClickMap3()
    {
        mapSelection = 2;
        UpdateImageMap();
    }
    
    public void OnClickMap4()
    {
        mapSelection = 3;
        UpdateImageMap();
    }
    
    public void OnClickStartSolo()
    {
        if (PhotonNetwork.CurrentRoom != null)
        {
            PhotonNetwork.LoadLevel(ListMap[mapSelection]);
        }
    }

    private void UpdateImageMap()
    {
        mapImage.texture = ListMapImage[mapSelection];
    }


    //--------------------------Solo VS IA------------------------

    public RawImage mapImageIA;
    
    public string[] ListMapIA;

    private int mapSelectionIA = 0;
    public void OnClickMap1IA()
    {
        mapSelectionIA = 0;
        UpdateImageMapIA();
    }

    public void OnClickMap2IA()
    {
        mapSelectionIA = 1;
        UpdateImageMapIA();
    }

    public void OnClickMap3IA()
    {
        mapSelectionIA = 2;
        UpdateImageMapIA();
    }

    public void OnClickMap4IA()
    {
        mapSelectionIA = 3;
        UpdateImageMapIA();
    }
    
    public void OnClickStartIA()
    {
        if (PhotonNetwork.CurrentRoom != null)
        {
            PhotonNetwork.LoadLevel(ListMapIA[mapSelection]);
        }
    }
    
    private void UpdateImageMapIA()
    {
        mapImageIA.texture = ListMapImage[mapSelectionIA];
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
    
    public GameObject MasterPanel;
    public RawImage mapImageMult;
    public int mapSelectMult = 0;

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
            MasterPanel.SetActive(true);
        }
        lobbyPanel.SetActive(false);
        roomPanel.SetActive(true);
        UserName.text = "";
        roomName.text = "Room Name : " + PhotonNetwork.CurrentRoom.Name;
        UpdatePlayerList();
        if (PhotonNetwork.IsMasterClient)
        {
            UpdateMultMapImage(0);
        }
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
        View.RPC("UpdateMultMapImage",RpcTarget.All,mapSelectMult);
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
        PhotonNetwork.LoadLevel(ListMap[mapSelectMult]);
    }

    public PhotonView View;
    public void OnClickMap1Mult()
    {
        mapSelectMult = 0;
        View.RPC("UpdateMultMapImage",RpcTarget.All,0);
    }

    
    public void OnClickMap2Mult()
    {
        mapSelectMult = 1;
        View.RPC("UpdateMultMapImage",RpcTarget.All,1);
    }

    public void OnClickMap3Mult()
    {
        mapSelectMult = 2;
        View.RPC("UpdateMultMapImage",RpcTarget.All,2);
    }

    public void OnClickMap4Mult()
    {
        mapSelectMult = 3;
        View.RPC("UpdateMultMapImage",RpcTarget.All,3);
    }
    
    [PunRPC]
    public void UpdateMultMapImage(int map)
    {
        mapImageMult.texture = ListMapImage[map];
    }
}
