using System.Collections;
using System.Collections.Generic;
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
    }
    public void OnClickIA()
    {
        MainPanel.SetActive(false);
        IAPanel.SetActive(true);
    }
    public void OnClickPVP()
    {
        MainPanel.SetActive(false);
        PVPPanel.SetActive(true);
        
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
        PhotonNetwork.CreateRoom(PhotonNetwork.NickName, new RoomOptions() { IsOpen = false, IsVisible = false });
        PhotonNetwork.LoadLevel("SampleScene");
    }
    
    
    
    //--------------------------Solo VS IA------------------------
    
    
    
    
    //--------------------------Multiplayer-----------------------

    public TMP_InputField RoomInputField;
    public GameObject lobbyPanel;
    public GameObject roomPanel;
    public TMP_Text roomName;
    public RoomItem RoomItemPrefab;
    private List<RoomItem> roomItemList = new List<RoomItem>();
    public Transform contentObject;
    

    public void OnClickCreate()
    {
        if (RoomInputField.text.Length >= 1)
        {
            PhotonNetwork.CreateRoom(RoomInputField.text, new RoomOptions() { MaxPlayers = 4 });
        }
    }

    public override void OnJoinedRoom()
    {
        lobbyPanel.SetActive(false);
        roomPanel.SetActive(true);
        UserName.text = "";
        roomName.text = "Room Name : " + PhotonNetwork.CurrentRoom.Name;
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        UpdateRoomList(roomList);
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
}
