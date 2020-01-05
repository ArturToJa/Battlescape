using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BattlescapeLogic;

public class NewTurnButton : MonoBehaviour
{
    Button thisButton;
    Text text;
    
    void Start()
    {
        thisButton = GetComponent<Button>();
        text = GetComponentInChildren<Text>();
        thisButton.onClick.AddListener(GameRound.instance.OnClick);
        TurnOff();
    }

    public void SetTextTo(string newText)
    {
        text.text = newText;
    }

    public void TurnOn()
    {
        UIManager.InstantlyTransitionActivity(this.gameObject, true);
    }

    public void TurnOff()
    {
        UIManager.InstantlyTransitionActivity(this.gameObject, false);
    }
    

}
