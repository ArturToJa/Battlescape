using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BattlescapeLogic;

public class NewTurnButton : MonoBehaviour
{
    private TurnChanger turnChanger;
    Button thisButton;
    Text text;
    
    protected void Start()
    {
        turnChanger = new TurnChanger(OnNewRound, OnNewTurn, OnNewPhase);
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

    public void OnNewRound()
    {
        return;
    }

    public void OnNewTurn()
    {
        return;
    }

    void SetActivity()
    {
        if (GameRound.instance.currentPlayer.IsCurrentLocalPlayer() && GameRound.instance.IsGameGoing())
        {
            TurnOn();
        }
        else
        {
            TurnOff();
        }
    }

    public void OnNewPhase()
    {
        switch (GameRound.instance.currentPhase)
        {
            case TurnPhases.None:
                break;
            case TurnPhases.Movement:
                SetActivity();
                SetTextTo("Next Phase");
                break;
            case TurnPhases.Attack:
                SetActivity();
                SetTextTo("End Turn!");
                break;
            case TurnPhases.Enemy:
                TurnOff();
                break;
            default:
                break;
        }
    }
}
