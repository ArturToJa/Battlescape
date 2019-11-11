using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using BattlescapeLogic;

public class MovementQuestions : MonoBehaviour
{

    public static MovementQuestions Instance { get; private set; }


    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void CheckIfAnyMoreUnitsToMove()
    {
        foreach (BattlescapeLogic.Unit ally in Global.instance.playerTeams[TurnManager.Instance.PlayerHavingTurn].players[0].playerUnits)
        {
            if (ally.CanStillMove())
            {
                return;
            }
        }
        PopupTextController.AddPopupText("No more units to move!", PopupTypes.Info);
    }

    public bool CanMove(BattlescapeLogic.Unit unit, Tile target)
    {
        if (
            Pathfinder.instance.IsLegalTileForUnit(MouseManager.Instance.mouseoveredTile, unit) &&
            IsItTimeToMove(unit) &&
            unit.CanStillMove()
           )
        {
            return true;
        }
        else
        {
            /* Debug.Log("Legal: " + IsThisTileLegal(target, unit isAffectedByCombat));
             Debug.Log("TimeToMove: " + IsItTimeToMove(unit));
             Debug.Log("HaveMovement: " + CanUnitMoveAtAll(unit.GetComponent<BattlescapeLogic.Unit>()));
             Debug.Log("LastTIleIsSafe: " + IsTheLastTileSafe());
             Debug.Log("UnitNotInCombat: " + !unit.CheckIfIsInCombat());*/
            return false;
        }
    }

    bool IsItTimeToMove(BattlescapeLogic.Unit unit)
    {
        if (unit.owner == Global.instance.playerTeams[TurnManager.Instance.PlayerToMove].players[0] &&
            (TurnManager.Instance.CurrentPhase == TurnPhases.Movement && GameStateManager.Instance.GameState != GameStates.TargettingState) || Ability_Basic.IsForcingMovementStuff)
        {
            return true;
        }
        else
        {
            return false;
        }
    }    
}
