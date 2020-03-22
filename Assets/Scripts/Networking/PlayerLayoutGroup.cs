using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Realtime;

public class PlayerLayoutGroup : MonoBehaviour
{
    [SerializeField]
    GameObject PlayerPrefab;

    [SerializeField] GameObject RoomObj;


    List<PlayerOnTheList> PlayerList = new List<PlayerOnTheList>();

    void OnJoinedRoom()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        RoomObj.transform.SetAsLastSibling();
        Player[] photonPlayers = PhotonNetwork.PlayerList;
        for (int i = 0; i < photonPlayers.Length; i++)
        {
            PlayerJoinedRoom(photonPlayers[i]);
        }
    }

    void OnPhotonPlayerConnected(Player photonPlayer)
    {
        PlayerJoinedRoom(photonPlayer);
    }

    void OnPhotonPlayerDisconnected(Player photonPlayer)
    {
        PlayerLeftRoom(photonPlayer);
    }

    void PlayerJoinedRoom(Player photonPlayer)
    {
        if (photonPlayer == null)
        {
            return;
        }
        PlayerLeftRoom(photonPlayer);
        GameObject PlayerObj = Instantiate(PlayerPrefab);
        PlayerObj.transform.SetParent(transform, false);
        PlayerOnTheList potl = PlayerObj.GetComponent<PlayerOnTheList>();
        potl.ApplyPhotonPlayer(photonPlayer);
        PlayerList.Add(potl);
        if (photonPlayer != PhotonNetwork.LocalPlayer)
        {
            FindObjectOfType<AudioManager>().Play("PlayerJoinedSound");
        }
    }

    void PlayerLeftRoom(Photon.Realtime.Player photonPlayer)
    {
        int index = PlayerList.FindIndex(x => x.PhotonPlayer == photonPlayer);
        if (index != -1)
        {
            Destroy(PlayerList[index].gameObject);
            PlayerList.RemoveAt(index);
        }        
    }

    public void OnClickLeaveRoom()
    {
        MyNetworkManager.Instance.Lobby.transform.SetAsLastSibling();

        PhotonNetwork.LeaveRoom();
    }
}
