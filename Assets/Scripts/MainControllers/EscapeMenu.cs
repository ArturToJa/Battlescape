using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using BattlescapeLogic;

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
        if (NetworkMessageSender.Instance.IsConnected())
        {
            if (SceneManager.GetActiveScene().name.Contains("_GameScene_"))
            {
                NetworkMessageSender.Instance.SendInfoToOthersThatDisconnected();
            }
            NetworkMessageSender.Instance.Disconnect();
        }

        FindObjectOfType<LevelLoader>().CommandLoadScene("_MENU");
    }

    public void Quit()
    {
        if (NetworkMessageSender.Instance.IsConnected())
        {
            if (SceneManager.GetActiveScene().name.Contains("_GameScene_"))
            {
                NetworkMessageSender.Instance.SendInfoToOthersThatDisconnected();
            }
            NetworkMessageSender.Instance.Disconnect();
        }
        Application.Quit();
    }
}
