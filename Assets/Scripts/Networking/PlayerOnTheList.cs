using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerOnTheList : MonoBehaviour
{

    public PhotonPlayer PhotonPlayer { get; private set; }

    public Text PlayerName;

    public void ApplyPhotonPlayer(PhotonPlayer photonPlayer)
    {
        PlayerName.text = photonPlayer.NickName;
        PhotonPlayer = photonPlayer;
    }
}
