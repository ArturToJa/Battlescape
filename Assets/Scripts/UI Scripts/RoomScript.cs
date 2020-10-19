using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using BattlescapeLogic;
using Photon.Pun;

public class RoomScript : MonoBehaviour
{
    PhotonView photonView;
    [SerializeField] Text RoomName;
    string message;
    [SerializeField] InputField inputField;
    [SerializeField] GameObject StartGameButton;
    [SerializeField] GameObject ChooseMapButton;
    bool isMatchStarted = false;
    string gameSceneName;
    [SerializeField] GameObject MapChoiceWindow;

    void Start()
    {
        photonView = GetComponent<PhotonView>();
    }
    void Update()
    {
        if (PhotonNetwork.CurrentRoom != null)
        {
            RoomName.text = PhotonNetwork.CurrentRoom.Name;
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
            (PhotonNetwork.IsMasterClient 
            && (PhotonNetwork.CurrentRoom.PlayerCount == 2)
            && Global.instance.HaveAllPlayersChosenRace());
        ChooseMapButton.SetActive
            (PhotonNetwork.IsMasterClient
            && PhotonNetwork.CurrentRoom.PlayerCount == 2
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
            Networking.instance.SendCommandToLoadScene(gameSceneName);
        }
        
    }

    public void ChooseMap()
    {
        MapChoiceWindow.transform.SetAsLastSibling();
    }

    public void SetMapToCurrentNumber()
    {
        // NOTE that names of the buttons currently need to correspond to the names of the scenes ;D bad code i know right
        photonView.RPC("RPCSetGameSceneName", RpcTarget.All, EventSystem.current.currentSelectedGameObject.name);
        Log.LobbySpawnLog("MapChosen: " + gameSceneName);
        transform.SetAsLastSibling();

    }

    [PunRPC]
    void RPCSetGameSceneName(string name)
    {
        gameSceneName = name;
    }
}
