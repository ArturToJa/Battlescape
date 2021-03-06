﻿using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BattlescapeSound;

namespace BattlescapeLogic
{
    public class LogConsole : MonoBehaviour
    {
        public static LogConsole instance;
        GameObject logPrefab;
        GameObject networkLogPrefab;
        Transform theConsole;
        PhotonView photonView;

        void OnEnable()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
                return;
            }
            logPrefab = (GameObject)Resources.Load("ConsolLogPrefab");
            networkLogPrefab = (GameObject)Resources.Load("NetworkConsolLogPrefab");
            theConsole = this.transform;
            photonView = this.GetComponent<PhotonView>();
        }

        public void SpawnLog(string log)
        {
            var temp = Instantiate(logPrefab, theConsole);
            temp.GetComponentInChildren<Text>().text = log;
        }

        public void NetworkSpawnLog(string log)
        {
            photonView.RPC("RPCNetworkSpawnLog", RpcTarget.All, log);
        }
        public void LobbySpawnLog(string log)
        {
            photonView.RPC("RPCLobbySpawnLog", RpcTarget.All, log);
        }

        public void LobbySpawnLog(string log, string sound)
        {
            photonView.RPC("RPCLobbySpawnLog", RpcTarget.All, log, sound);
        }

        [PunRPC]
        public void RPCNetworkSpawnLog(string log)
        {
            var temp = Instantiate(logPrefab, theConsole);
            temp.GetComponentInChildren<Text>().text = log;
            SoundManager.instance.PlaySound(this.gameObject, SoundManager.instance.logSound);
        }
        [PunRPC]
        public void RPCLobbySpawnLog(string log)
        {
            var temp = Instantiate(networkLogPrefab, theConsole);
            temp.GetComponentInChildren<Text>().text = log;
            SoundManager.instance.PlaySound(this.gameObject, SoundManager.instance.lobbySound);
        }
    }
}
