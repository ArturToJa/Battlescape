using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

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
        foreach (UnitScript ally in Player.Players[TurnManager.Instance.PlayerHavingTurn].PlayerUnits)
        {
            if (CanUnitMoveAtAll(ally.GetComponent<UnitMovement>()))
            {
                return;
            }
        }
        PopupTextController.AddPopupText("No more units to move!", PopupTypes.Info);
    }

   /* bool IsQC()
    {
        return UnitToMove.GetComponent<UnitScript>().CheckIfIsInCombat()
                    && GameStateManager.Instance.IsCurrentPlayerAI() == false
                    && TurnManager.Instance.CurrentPhase == TurnPhases.Movement
                    && GameStateManager.Instance.IsCurrentPlayerLocal();
    }*/

   /* void DoQC()
    {
        int x = Mathf.RoundToInt(temporaryPositions[temporaryPositions.Count - 1].transform.position.x);
        int z = Mathf.RoundToInt(temporaryPositions[temporaryPositions.Count - 1].transform.position.z);
        if (GameStateManager.Instance.MatchType == MatchTypes.Online)
        {
            photonView.RPC("RPCStartQC", PhotonTargets.All, x, z);
        }
        else
        {
            QCManager.Instance.StartQC(Map.Board[x, z]);
        }
    }*/

    [PunRPC]
    void RPCStartQC(int x, int z)
    {
        QCManager.Instance.StartQC(Map.Board[x, z]);
    }





    public bool CanMove(UnitScript unit, Tile target, bool isAffectedByCombat)
    {
        if (
            IsThisTileLegal(target, unit.GetComponent<UnitMovement>(), isAffectedByCombat) &&
            IsItTimeToMove(unit) &&
            CanUnitMoveAtAll(unit.GetComponent<UnitMovement>())
           )
        {
            return true;
        }
        else
        {
            /* Debug.Log("Legal: " + IsThisTileLegal(target, unit.GetComponent<UnitMovement>(), isAffectedByCombat));
             Debug.Log("TimeToMove: " + IsItTimeToMove(unit));
             Debug.Log("HaveMovement: " + CanUnitMoveAtAll(unit.GetComponent<UnitMovement>()));
             Debug.Log("LastTIleIsSafe: " + IsTheLastTileSafe());
             Debug.Log("UnitNotInCombat: " + !unit.CheckIfIsInCombat());*/
            return false;
        }
    }

    bool IsItTimeToMove(UnitScript unit)
    {
        if (unit.PlayerID == TurnManager.Instance.PlayerToMove &&
            (TurnManager.Instance.CurrentPhase == TurnPhases.Movement) || Ability_Basic.IsForcingMovementStuff)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool CanUnitMoveAtAll(UnitMovement unit)
    {

        return (unit.CanMove && unit.GetComponent<UnitScript>().IsFrozen == false);
    }

    bool AreTheTilesNeighbours(Tile a, Tile b)
    {
        if (a.GetNeighbours().Contains(b))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool IsTheLastTileSafe(UnitScript unit)
    {
        if (PathCreator.Instance.Path.Count <= 1)
        {
            return true;
        }
        Tile lastTile = PathCreator.Instance.Path[PathCreator.Instance.Path.Count - 1];
        if (lastTile.TCTool.IsTileSafeFor(lastTile, unit))
        {
            return true;
        }
        else
        {
            return false;
        }
    }



    bool CheckIfReturnImmeediately()
    {
        if (MouseManager.Instance.SelectedUnit == null || EventSystem.current.IsPointerOverGameObject())
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    bool IsThisTileLegal(Tile tile, UnitMovement unit, bool isAffectedByCombat)
    {
        Pathfinder.Instance.BFS(unit.GetComponent<UnitScript>().myTile, true);
        if (Pathfinder.Instance.GetAllLegalTilesAndIfTheyAreSafe(unit, isAffectedByCombat).ContainsKey(tile))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    bool CanQuitCombat(UnitScript unit)
    {
        if (
            unit.CheckIfIsInCombat() &&
            IsItTimeToMove(unit) &&
            unit.GetComponent<UnitMovement>().CanMove &&
            IsThisTileLegal(MouseManager.Instance.mouseoveredTile, unit.GetComponent<UnitMovement>(), true)
            )
        {
            return true;
        }
        else
        {
            /*Debug.Log("inCombat: " + unit.CheckIfIsInCombat());
            Debug.Log("timetoMove: " + IsItTimeToMove(unit));
            Debug.Log("canMove: " + unit.GetComponent<UnitMovement>().canMove);
            Debug.Log("LegalTile: " + IsItALegalTileForQC(unit, MouseManager.Instance.mouseoveredTile));*/
            return false;
        }
    }
}
