using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;using BattlescapeLogic;

public class PhaseInfo : MonoBehaviour {

    public Text PhaseInfoText;

    void Update()
    {
        if (TurnManager.Instance.TurnCount <= 0)
        {
            PhaseInfoText.text = "Positioning Phase";
            return;
        }
        if (TurnManager.Instance.CurrentPhase == TurnPhases.Enemy)
        {
            PhaseInfoText.text = Global.instance.GetNextPlayer(Global.instance.playerTeams[TurnManager.Instance.PlayerHavingTurn].players[0]).playerName.ToString() + "'s Responding!";
        }
        else
        {
            if (TurnManager.Instance.CurrentPhase == TurnPhases.Movement)
            {
                PhaseInfoText.text = "Movement Phase";
            }
            if (TurnManager.Instance.CurrentPhase == TurnPhases.Shooting)
            {
                PhaseInfoText.text = "Shooting Phase";
            }
            if (TurnManager.Instance.CurrentPhase == TurnPhases.Attack)
            {
                PhaseInfoText.text = "Attack Phase";
            }

        }

    }
}
