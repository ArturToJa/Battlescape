using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BattlescapeLogic;

public class PhaseInfo : MonoBehaviour {

    public Text PhaseInfoText;

    void Update()
    {
        if (GameRound.instance.gameRoundCount <= 0)
        {
            PhaseInfoText.text = "Positioning Phase";
            return;
        }
        if (GameRound.instance.currentPhase == TurnPhases.Enemy)
        {
            PhaseInfoText.text = Global.instance.GetNextPlayer(GameRound.instance.currentPlayer).playerName.ToString() + "'s Responding!";
        }
        else
        {
            if (GameRound.instance.currentPhase == TurnPhases.Movement)
            {
                PhaseInfoText.text = "Movement Phase";
            }
            if (GameRound.instance.currentPhase == TurnPhases.Attack)
            {
                PhaseInfoText.text = "Attack Phase";
            }

        }

    }
}
