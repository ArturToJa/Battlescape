using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattlescapeLogic
{
    public class PunNetworking : NetworkingBaseClass
    {
        public override void Connect()
        {
            PhotonNetwork.ConnectUsingSettings();
        }

        public override void Disconnect()
        {
            Debug.Log("Disconnected!");
            PhotonNetwork.Disconnect();
        }

        public override bool IsConnected()
        {
            return PhotonNetwork.IsConnected;
        }

        public override void JoinRoom(string roomName)
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
}
