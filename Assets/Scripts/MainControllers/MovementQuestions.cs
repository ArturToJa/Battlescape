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
        foreach (UnitScript ally in Global.instance.playerTeams[TurnManager.Instance.PlayerHavingTurn].players[0].playerUnits)
        {
            if (CanUnitMoveAtAll(ally.GetComponent<UnitScript>()))
            {
                return;
            }
        }
        PopupTextController.AddPopupText("No more units to move!", PopupTypes.Info);
    }

    public bool CanMove(UnitScript unit, Tile target)
    {
        if (
            Pathfinder.instance.IsTileLegalForUnit(MouseManager.Instance.mouseoveredTile, unit) &&
            IsItTimeToMove(unit) &&
            CanUnitMoveAtAll(unit.GetComponent<UnitScript>())
           )
        {
            return true;
        }
        else
        {
            /* Debug.Log("Legal: " + IsThisTileLegal(target, unit isAffectedByCombat));
             Debug.Log("TimeToMove: " + IsItTimeToMove(unit));
             Debug.Log("HaveMovement: " + CanUnitMoveAtAll(unit.GetComponent<UnitScript>()));
             Debug.Log("LastTIleIsSafe: " + IsTheLastTileSafe());
             Debug.Log("UnitNotInCombat: " + !unit.CheckIfIsInCombat());*/
            return false;
        }
    }

    bool IsItTimeToMove(UnitScript unit)
    {
        if (unit.PlayerID == Global.instance.playerTeams[TurnManager.Instance.PlayerToMove].players[0].team.index &&
            (TurnManager.Instance.CurrentPhase == TurnPhases.Movement && GameStateManager.Instance.GameState != GameStates.TargettingState) || Ability_Basic.IsForcingMovementStuff)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool CanUnitMoveAtAll(UnitScript unit)
    {

        return (unit.CanStillMove() && unit.GetComponent<UnitScript>().IsFrozen == false);
    }   
}
