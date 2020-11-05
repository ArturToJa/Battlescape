using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;

public class PlayerOnTheList : MonoBehaviour
{

    public Player PhotonPlayer { get; private set; }

    public Text PlayerName;

    public void ApplyPhotonPlayer(Player photonPlayer)
    {
        PlayerName.text = photonPlayer.NickName;
        PhotonPlayer = photonPlayer;
    }
}
