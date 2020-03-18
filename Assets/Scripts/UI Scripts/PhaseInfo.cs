using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BattlescapeLogic;

public class PhaseInfo : TurnChangeMonoBehaviour
{

    [SerializeField] Text phaseInfoText;

    public override void OnCreation()
    {
        base.OnCreation();    
        phaseInfoText.text = "Positioning Phase";        
    }

    public void OnEnemyPhase(Player enemy)
    {
        phaseInfoText.text = enemy.playerName.ToString() + "'s Responding!";
    }

    public override void OnNewPhase()
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

    public override void OnNewRound()
    {
        return;
    }

    public override void OnNewTurn()
    {
        return;
    }
}
