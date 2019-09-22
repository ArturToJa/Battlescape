using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerNameSetter : MonoBehaviour
{
    [SerializeField] Button OK;
    [SerializeField] InputField inputField;

    void Start()
    {
        if ((PlayerPrefs.HasKey("MyPlayerName")))
        {
            Okay();
        }
    }
    
    void Update()
    {
        OK.interactable = (inputField.text.Length > 0);
    }

    public void SetName(string input)
    {
        if (input != null)
        {
            PlayerPrefs.SetString("MyPlayerName", input);            
        }
    }

    //this is both on a button and in start, if we already have a name. For now. In the future i might build a morel ogical system where you can maybe login to the lobby with password and the system will remember your stats and so on, for now it is just a simple game...
    public void Okay()
    {
        PhotonNetwork.playerName = "#" + Random.Range(1000,10000) + " " + PlayerPrefs.GetString("MyPlayerName");             
        MyNetworkManager.Instance.Lobby.transform.SetAsLastSibling();
    }
}
