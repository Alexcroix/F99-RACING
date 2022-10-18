using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerSpawner : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private GameObject[] PlayerPrefabs;

    [SerializeField]
    private GameObject[] PlayerSpawners;

    void Start()
    {
        int i = (int)PhotonNetwork.LocalPlayer.CustomProperties["NumberInRoom"];
        
        PhotonNetwork.Instantiate(PlayerPrefabs[i].name, PlayerSpawners[i].transform.position, Quaternion.Euler(0,90,0));
    }
}
