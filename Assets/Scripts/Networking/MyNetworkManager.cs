using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MyNetworkManager : MonoBehaviour
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
        PhotonNetwork.ConnectUsingSettings(PlayerPrefs.GetString("Version"));
    } 

    void OnPhotonPlayerDisconnected(PhotonPlayer player)
    {
        if (GameStateManager.Instance.IsSceneGameScene(SceneManager.GetActiveScene()))
        {
            GameStateManager.Instance.GetComponent<PhotonView>().RPC("RPCConnectionLossScreen", PhotonTargets.All, "Player " + player.NickName + " lost connection or ragequitted. You win :( We are very sorry for that!");
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
        PhotonNetwork.JoinLobby(TypedLobby.Default);
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
