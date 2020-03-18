using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WindowSetter : MonoBehaviour
{

    [SerializeField] GameObject startingScreen;
    [HideInInspector] public GameObject currentScreen;

    void Start()
    {
        currentScreen = startingScreen;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && currentScreen == startingScreen && SceneManager.GetActiveScene().name == "_ManagementScene")
        {
            GoBack();
        }
    }

    public void GoBack()
    {
        SaveLoadManager.instance.currentSaveName = null;
        currentScreen = startingScreen;
        FindObjectOfType<LevelLoader>().CommandLoadScene("_MENU");
        if (PhotonNetwork.connected)
        {
            MyNetworkManager.Instance.Disconnect();
        }
    }
}
