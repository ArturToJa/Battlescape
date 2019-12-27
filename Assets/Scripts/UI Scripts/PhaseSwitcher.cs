using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BattlescapeLogic;

public class PhaseSwitcher : MonoBehaviour
{

    void Start()
    {
        this.gameObject.SetActive(true);
    }

    void LateUpdate()
    {
        if (Global.instance.IsCurrentPlayerAI() || TurnManager.Instance.IsItPreGame())
        {
            return;
        }
        if (TurnManager.Instance.CurrentPhase == TurnPhases.Movement && CanAnyoneElseMove() == false)
        {
            TurnManager.Instance.NextPhase(false);
        }
    }

    bool CanAnyoneElseMove()
    {
        foreach (Unit ally in Global.instance.playerTeams[TurnManager.Instance.PlayerHavingTurn].players[0].playerUnits)
        {
            if (ally.CanStillMove())
            {
                return true;
            }
        }
        return false;
    }
}
