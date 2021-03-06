﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using BattlescapeLogic;
using Photon.Pun;
using Photon.Realtime;

public class MyNetworkManager : MonoBehaviourPunCallbacks
{
    public GameObject Lobby
    {
        get
        {
            return GameObject.FindGameObjectWithTag("Lobby");
        }
    }

    


    public static MyNetworkManager Instance { get; set; }

    void Start()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }    

    public void Connect()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);
    }
    void OnPhotonPlayerDisconnected(Photon.Realtime.Player player)
    {
        if (SceneManager.GetActiveScene().name.Contains("_GameScene_"))
        {
            Networking.instance.photonView.RPC("RPCConnectionLossScreen", RpcTarget.All, "Player " + player.NickName + " lost connection or ragequitted. You win :( We are very sorry for that!");
        }
    }

    public void Disconnect()
    {
        Debug.Log("Disconnected!");
        PhotonNetwork.Disconnect();
    }
    void OnJoinedLobby()
    {
        Debug.Log("Joined lobby.");
    }

    void OnConnectedToMaster()
    {
        Debug.Log("Connected to master!");
        PhotonNetwork.JoinLobby(Photon.Realtime.TypedLobby.Default);
    }


    public void OnClickJoinRoom(string roomName)
    {
        if (PhotonNetwork.JoinRoom(roomName))
        {

        }
        else
        {
            Debug.Log("Failed to join the room!");
        }
    }

    

}
