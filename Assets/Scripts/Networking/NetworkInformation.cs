using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NetworkInformation : MonoBehaviour
{
    [SerializeField] Text infoText;


    void Update()
    {
        infoText.text = PhotonNetwork.NetworkClientState.ToString();
    }
}
