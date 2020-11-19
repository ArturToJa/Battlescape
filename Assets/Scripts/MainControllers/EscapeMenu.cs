using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using BattlescapeLogic;
using Photon.Pun;

public class EscapeMenu : MonoBehaviour
{
    [SerializeField] GameObject EscapeMenuWindow;
    [SerializeField] GameObject SureToQuitWindow;
    [SerializeField] GameObject SureToGoToMainMenuWindow;
    AudioSource source;

    void Start()
    {
        source = gameObject.AddComponent<AudioSource>();
        source.clip = Resources.Load<AudioClip>("EscapeMenuSound");
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && (FindObjectOfType<EnemyTooltipHandler>() == null || (FindObjectOfType<EnemyTooltipHandler>() != null)))
        {
            EscapeMenuWindow.SetActive(!EscapeMenuWindow.activeSelf);
            source.Play();
        }
    }
    public void MainMenu()
    {
        if (PhotonNetwork.IsConnected)
        {
            if (SceneManager.GetActiveScene().name.Contains("_GameScene_"))
            {
                Networking.instance.photonView.RPC("RPCConnectionLossScreen", RpcTarget.Others, PhotonNetwork.LocalPlayer.NickName);
            }
            MyNetworkManager.Instance.Disconnect();
        }

        Networking.instance.SendCommandToLoadScene("_MENU");
    }

    public void Quit()
    {
        if (PhotonNetwork.IsConnected)
        {
            if (SceneManager.GetActiveScene().name.Contains("_GameScene_"))
            {
                Networking.instance.photonView.RPC("RPCConnectionLossScreen", RpcTarget.Others, PhotonNetwork.LocalPlayer.NickName);
            }
            MyNetworkManager.Instance.Disconnect();
        }
        Application.Quit();
    }
}
