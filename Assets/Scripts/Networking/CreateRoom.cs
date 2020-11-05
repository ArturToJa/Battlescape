using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateRoom : MonoBehaviour
{
    [SerializeField]
    Text _roomName;
    public Text RoomName
    {
        get
        {
            return _roomName;
        }
    }

    void Update()
    {
        GetComponent<Button>().interactable = !(string.IsNullOrEmpty(RoomName.text));    
    }

    public void OnClick_CreateRoom()
    {
        RoomOptions roomOptions = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = 2 };
        if (PhotonNetwork.CreateRoom(RoomName.text, roomOptions,TypedLobby.Default))
        {
            Debug.Log("Create room succesfully sent");
        }
        else
        {
            Debug.LogWarning("Create room failed to send");
        }
    }
    void OnPhotonCreateRoomFailed(object[] codeAndMessage)
    {
        Debug.Log("Creating room failed: " + codeAndMessage);
    }

    void OnCreatedRoom()
    {
        
    }
}
