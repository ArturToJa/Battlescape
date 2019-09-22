using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class RoomScript : MonoBehaviour
{
    PhotonView photonView;
    [SerializeField] Text RoomName;
    string message;
    [SerializeField] InputField inputField;
    [SerializeField] GameObject StartGameButton;
    [SerializeField] GameObject ChooseMapButton;
    bool isMatchStarted = false;
    string GameSceneName;
    [SerializeField] GameObject MapChoiceWindow;

    void Start()
    {
        photonView = GetComponent<PhotonView>();
    }
    void Update()
    {
        if (PhotonNetwork.room != null)
        {
            RoomName.text = PhotonNetwork.room.Name;
        }        
        if (Input.GetKeyDown(KeyCode.Return) && string.IsNullOrEmpty(message) == false)
        {
            Log.LobbySpawnLog(PlayerPrefs.GetString("MyPlayerName") + ": " + message);
            inputField.text = string.Empty;
            inputField.ActivateInputField();
        }
        if (Input.GetKeyDown(KeyCode.Tab))
        {
           inputField.ActivateInputField();
        }
        StartGameButton.SetActive
            (PhotonNetwork.isMasterClient 
            && (PhotonNetwork.room.PlayerCount == 2)
            && SaveLoadManager.Instance.AreBothFactionsChosen);
        ChooseMapButton.SetActive
            (PhotonNetwork.isMasterClient
            && PhotonNetwork.room.PlayerCount == 2
            );
    }

    public void OnInputEnd(string msg)
    {
        message = msg;
    }    

    public void OnClickStartMatch()
    {
        if (isMatchStarted == false)
        {
            isMatchStarted = true;
            photonView.RPC("RPCSetPlayerTypes", PhotonTargets.All);
            FindObjectOfType<LevelLoader>().CommandLoadScene(GameSceneName);
        }
        
    }

    public void ChooseMap()
    {
        MapChoiceWindow.transform.SetAsLastSibling();
    }

    public void SetMapToCurrentNumber()
    {
        // NOTE that names of the buttons currently need to correspond to the names of the scenes ;D bad code i know right
        photonView.RPC("RPCSetGameSceneName", PhotonTargets.All, EventSystem.current.currentSelectedGameObject.name);
        Log.LobbySpawnLog("MapChosen: " + GameSceneName);
        transform.SetAsLastSibling();

    }

    [PunRPC]
    void RPCSetPlayerTypes()
    {
        //this function sets playertypes, so tells us who is "mylocalplayer" ;) If we are the MasterClient, then we are player 0 (so players[0] is Local) and if not then we are player 1. 
        if (PhotonNetwork.isMasterClient)
        {
            Player.Players[0].Type = PlayerType.Local;
            Player.Players[1].Type = PlayerType.Network;
        }
        else
        {
            Player.Players[0].Type = PlayerType.Network;
            Player.Players[1].Type = PlayerType.Local;        
        }
    }

    [PunRPC]
    void RPCSetGameSceneName(string name)
    {
        GameSceneName = name;
    }
}
