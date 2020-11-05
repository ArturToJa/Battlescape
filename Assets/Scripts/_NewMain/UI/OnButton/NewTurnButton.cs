using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BattlescapeLogic;

public class NewTurnButton : TurnChangeMonoBehaviour
{
    Button thisButton;
    Text text;
    
    protected override void Start()
    {
        base.Start();
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

    public override void OnNewRound()
    {
        return;
    }

    public override void OnNewTurn()
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

    public override void OnNewPhase()
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
