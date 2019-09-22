using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            if (TurnManager.Instance.CurrentPhase == TurnPhases.Shooting)
            {
                TurnManager.Instance.NextPhase(false);
            }
            else
            {
                TurnManager.Instance.NewTurn(true);
            }
        }
    }

    bool CanAnyoneElseMove()
    {
        foreach (UnitScript ally in Player.Players[TurnManager.Instance.PlayerHavingTurn].PlayerUnits)
        {
            if (MovementQuestions.Instance.CanUnitMoveAtAll(ally.GetComponent<UnitMovement>()))
            {
                return true;
            }
        }
        return false;
    }
}
