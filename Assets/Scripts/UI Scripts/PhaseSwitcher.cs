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
        if (GameStateManager.Instance.IsCurrentPlayerAI() || GameStateManager.Instance.IsItPreGame() || TurnManager.Instance.TurnCount < 1)
        {
            return;
        }
        if (TurnManager.Instance.CurrentPhase == TurnPhases.Movement && CanAnyoneElseMove() == false)
        {
            TurnManager.Instance.NextPhase(false);
        }
        else if (CombatController.CheckIfLastAttacker())
        {
            TurnManager.Instance.NewTurn(true);
        }
    }

    bool CanAnyoneElseMove()
    {
        foreach (BattlescapeLogic.Unit ally in Global.instance.playerTeams[TurnManager.Instance.PlayerHavingTurn].players[0].playerUnits)
        {
            if (ally.CanStillMove())
            {
                return true;
            }
        }
        return false;
    }
}
