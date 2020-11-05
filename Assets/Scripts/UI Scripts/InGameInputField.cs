using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BattlescapeLogic;
using Photon.Pun;

public class InGameInputField : MonoBehaviour
{
    public string msg;
    // Use this for initialization
    void Start()
    {
        msg = "";
        if (Global.instance.matchType != MatchTypes.Online)
        {
            this.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (string.IsNullOrEmpty(msg) == false && Input.GetKeyDown(KeyCode.Return))
        {
            LogConsole.instance.NetworkSpawnLog(PhotonNetwork.NickName + ": " + msg);
            this.GetComponent<InputField>().text = string.Empty;
            this.GetComponent<InputField>().ActivateInputField();
        }
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            this.GetComponent<InputField>().ActivateInputField();
        }
    }

    public void OnEdit(string s)
    {
        msg = s;
    }

    public static bool IsNotTypingInChat()
    {
        InGameInputField temp = FindObjectOfType<InGameInputField>();
        if (temp == null)
        {
            return true;
        }
        return temp.GetComponent<InputField>().isFocused == false;
    }
}
