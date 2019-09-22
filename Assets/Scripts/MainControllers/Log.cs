using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Log : MonoBehaviour
{
    static GameObject logPrefab;
    static GameObject networkLogPrefab;
    static Transform theConsole;
    static PhotonView photonView;

    void OnEnable()
    {
        logPrefab = (GameObject)Resources.Load("ConsolLogPrefab");
        networkLogPrefab = (GameObject)Resources.Load("NetworkConsolLogPrefab");
        theConsole = this.transform;
        photonView = this.GetComponent<PhotonView>();
    }

    public static void SpawnLog(string log)
    {
        var temp = Instantiate(logPrefab, theConsole);
        temp.GetComponentInChildren<Text>().text = log;
    }

    public static void NetworkSpawnLog(string log)
    {
        photonView.RPC("RPCNetworkSpawnLog", PhotonTargets.All, log);
    }
    public static void LobbySpawnLog(string log)
    {
        photonView.RPC("RPCLobbySpawnLog", PhotonTargets.All, log);
    }

    public static void LobbySpawnLog(string log, string sound)
    {
        photonView.RPC("RPCLobbySpawnLog", PhotonTargets.All, log, sound);
    }

    [PunRPC]
    public void RPCNetworkSpawnLog(string log)
    {
        var temp = Instantiate(logPrefab, theConsole);
        temp.GetComponentInChildren<Text>().text = log;
        FindObjectOfType<AudioManager>().Play("LogSound");
    }
    [PunRPC]
    public void RPCLobbySpawnLog(string log)
    {
        var temp = Instantiate(networkLogPrefab, theConsole);
        temp.GetComponentInChildren<Text>().text = log;
        FindObjectOfType<AudioManager>().Play("LogSound");
    }
    [PunRPC]
    public void RPCLobbySpawnLog(string log, string sound)
    {
        var temp = Instantiate(networkLogPrefab, theConsole);
        temp.GetComponentInChildren<Text>().text = log;
        FindObjectOfType<AudioManager>().Play(sound);
    }
}
