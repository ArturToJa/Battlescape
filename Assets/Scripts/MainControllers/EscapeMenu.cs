using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        if (Input.GetKeyDown(KeyCode.Escape) &&(FindObjectOfType<EnemyTooltipHandler>() == null || (FindObjectOfType<EnemyTooltipHandler>() != null && EnemyTooltipHandler.isOn == false)))
        {
            EscapeMenuWindow.SetActive(!EscapeMenuWindow.activeSelf);
            source.Play();
        }
    }
    public void MainMenu()
    {
        if (PhotonNetwork.connected)
        {
            if (GameStateManager.Instance.IsSceneGameScene(SceneManager.GetActiveScene()))
            {
                GameStateManager.Instance.GetComponent<PhotonView>().RPC("RPCConnectionLossScreen", PhotonTargets.Others, PhotonNetwork.player.NickName);
            }            
            MyNetworkManager.Instance.Disconnect();
        }
        
        FindObjectOfType<LevelLoader>().CommandLoadScene("_MENU");
    }

    public void Quit()
    {
        if (PhotonNetwork.connected)
        {
            if (GameStateManager.Instance.IsSceneGameScene(SceneManager.GetActiveScene()))
            {
                GameStateManager.Instance.GetComponent<PhotonView>().RPC("RPCConnectionLossScreen", PhotonTargets.Others, PhotonNetwork.player.NickName);
            }
            MyNetworkManager.Instance.Disconnect();
        }
        Application.Quit();
    }
}
