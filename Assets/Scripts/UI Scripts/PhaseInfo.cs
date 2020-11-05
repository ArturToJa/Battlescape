using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BattlescapeLogic;

public class PhaseInfo : MonoBehaviour
{
    TurnChanger turnChanger;
    [SerializeField] Text phaseInfoText;

    public void OnCreation()
    {
        turnChanger = new TurnChanger(OnNewRound, OnNewTurn, OnNewPhase);
        phaseInfoText.text = "Positioning Phase";        
    }

    public void OnEnemyPhase(Player enemy)
    {
        phaseInfoText.text = enemy.playerName.ToString() + "'s Responding!";
    }

    public void OnNewPhase()
    {
        switch (GameRound.instance.currentPhase)
        {
            case TurnPhases.None:
                break;
            case TurnPhases.Movement:
                phaseInfoText.text = "Movement Phase";
                break;
            case TurnPhases.Attack:
                phaseInfoText.text = "Attack Phase";
                break;
            case TurnPhases.Enemy:
                phaseInfoText.text = "Enemy is Responding!";
                break;
            case TurnPhases.All:
                break;
            default:
                break;
        }
    }

    public void OnNewRound()
    {
        return;
    }

    public void OnNewTurn()
    {
        return;
    }
}
