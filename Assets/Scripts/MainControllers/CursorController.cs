using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BattlescapeLogic;

public class CursorController : MonoBehaviour
{
    public static CursorController Instance { get; private set; }

    [SerializeField] List<Texture2D> MiddleHotspotCursors;

    [SerializeField] Texture2D defaultCursor;
    [SerializeField] Texture2D clickingDefaultCursor;
    [SerializeField] Texture2D waitingCursor;
    [SerializeField] Texture2D shootingCursor;
    [SerializeField] Texture2D clickingShootingCursor;
    [SerializeField] Texture2D badRangeShootingCursor;
    [SerializeField] Texture2D clickingBadRangeShootingCursor;
    [SerializeField] Texture2D attackCursor;
    [SerializeField] Texture2D clickingAttackCursor;
    [SerializeField] Texture2D walkingCursor;
    [SerializeField] Texture2D clickingWalkingCursor;
    [SerializeField] Texture2D invalidTargetCursor;
    [SerializeField] Texture2D validTargetCursor;
    [SerializeField] Texture2D clickingTargetCursor;
    [SerializeField] Texture2D infoCursor;
    [SerializeField] Texture2D selectionCursor;
    [SerializeField] Texture2D clickingSelectionCursor;
    [SerializeField] Texture2D blockedCursor;
    [SerializeField] Texture2D clickingBlockedCursor;
    [SerializeField] Texture2D enterCombatCursor;
    [SerializeField] Texture2D clickingEnterCombatCursor;
    [SerializeField] Texture2D QCMovementCursor;
    [SerializeField] Texture2D clickingQCMovementCursor;


    Texture2D previousCursor;
    Texture2D currentCursor;

    CursorMode curMode = CursorMode.Auto;
    Vector2 basicHotspot = Vector2.zero;
    Vector2 middleHotspot = new Vector2(32, 32);

    bool wasCursorForcedByUI = false;
    bool skipSettingCursor = false;

    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    void LateUpdate()
    {
        CheckForRetaliationState();
        CheckForIdleState();
        CheckForTargettingState();
        CheckForMoveState();
        CheckForAttackState();
        CheckForShootingState();
        CheckForBeingOverUI();
        CheckForAnimatingState();
        // CheckForQuitCombat();
    }

    private void CheckForShootingState()
    {
        if (skipSettingCursor)
        {
            return;
        }
        if (GameStateManager.Instance.GameState != GameStates.AttackState)
        {
            return;
        }
        if (MouseManager.Instance.SelectedUnit == null)
        {
            return;
        }
        if (MouseManager.Instance.SelectedUnit.IsRanged())
        {            
            if (MouseManager.Instance.MouseoveredUnit != null)
            {
                if (MouseManager.Instance.MouseoveredUnit.owner == MouseManager.Instance.SelectedUnit.owner)
                {
                    SetCursorTo(clickingSelectionCursor, selectionCursor);
                }
                else if (CombatController.Instance.PossibleToShootAt(MouseManager.Instance.SelectedUnit, MouseManager.Instance.MouseoveredUnit, true))
                {
                    SetCursorTo(clickingShootingCursor, shootingCursor);
                }
                else
                {
                    SetCursorTo(clickingBlockedCursor, blockedCursor);
                }
            }
            else if (MouseManager.Instance.IsMousoveredTile())
            {
                SetCursorTo(clickingBlockedCursor, blockedCursor);
            }
            else
            {
                SetCursorTo(clickingDefaultCursor, defaultCursor);
            }
        }        
    }

    private void CheckForRetaliationState()
    {
        if (skipSettingCursor)
        {
            return;
        }
        if (GameStateManager.Instance.GameState != GameStates.RetaliationState)
        {
            return;
        }
        SetCursorTo(clickingBlockedCursor, blockedCursor);
    }

    private void CheckForTargettingState()
    {
        if (skipSettingCursor)
        {
            return;
        }
        if (GameStateManager.Instance.GameState == GameStates.TargettingState)
        {
            if (GameStateManager.Instance.isTargetValid)
            {
                SetCursorTo(clickingTargetCursor, validTargetCursor);
            }
            else
            {
                SetCursorTo(clickingTargetCursor, invalidTargetCursor);
            }

        }
    }

    private void CheckForBeingOverUI()
    {
        if (skipSettingCursor)
        {
            return;
        }
        if (Helper.IsOverNonHealthBarUI() && GameStateManager.Instance.GameState != GameStates.AnimatingState)
        {
            previousCursor = currentCursor;
            if (Input.GetMouseButton(0))
            {
                Cursor.SetCursor(clickingDefaultCursor, GetHotspotType(clickingDefaultCursor), curMode);
            }
            else
            {
                Cursor.SetCursor(defaultCursor, GetHotspotType(defaultCursor), curMode);
            }
            wasCursorForcedByUI = true;

        }
        else if (wasCursorForcedByUI && GameStateManager.Instance.GameState != GameStates.AnimatingState)
        {
            wasCursorForcedByUI = false;
            SetCursorTo(previousCursor, previousCursor);
        }
    }

    private void CheckForAttackState()
    {
        if (skipSettingCursor || GameStateManager.Instance.GameState != GameStates.AttackState)
        {
            return;
        }

        //if (MouseManager.Instance.mouseoveredDestructible != null && MouseManager.Instance.SelectedUnit != null && MouseManager.Instance.SelectedUnit.CanAttackDestructible(MouseManager.Instance.mouseoveredDestructible.GetComponent<DestructibleScript>(), false))
        //{
        //    SetCursorTo(clickingAttackCursor, attackCursor);
        //    return;
        //}

        if (MouseManager.Instance.MouseoveredUnit != null && MouseManager.Instance.SelectedUnit != null)
        {
            if (MouseManager.Instance.SelectedUnit.currentPosition.neighbours.Contains(MouseManager.Instance.MouseoveredUnit.currentPosition) && MouseManager.Instance.SelectedUnit.CanStillAttack() && MouseManager.Instance.SelectedUnit.attack != null)
            {
                SetCursorTo(clickingAttackCursor, attackCursor);
            }
            else if (MouseManager.Instance.MouseoveredUnit.owner == Global.instance.playerTeams[TurnManager.Instance.PlayerToMove].players[0])
            {
                SetCursorTo(clickingSelectionCursor, selectionCursor);
            }
            else
            {
                SetCursorTo(clickingBlockedCursor, blockedCursor);
            }
        }
        else
        {
            SetCursorTo(clickingBlockedCursor, blockedCursor);
        }


    }

    private void CheckForMoveState()
    {
        if (skipSettingCursor || GameStateManager.Instance.GameState != GameStates.MoveState || MouseManager.Instance.SelectedUnit == null)
        {
            return;
        }

        if (MouseManager.Instance.MouseoveredUnit != null && MouseManager.Instance.MouseoveredUnit.IsAlive())
        {
            CheckWhatUnitAreWeOver(clickingSelectionCursor, selectionCursor, clickingBlockedCursor, blockedCursor);
        }
        else if (MouseManager.Instance.IsMousoveredTile())
        {

            CheckWhatTileAreWeOver();
        }
        else
            SetCursorTo(clickingDefaultCursor, defaultCursor);
    }

    private void CheckForIdleState()
    {
        if (skipSettingCursor)
        {
            return;
        }
        if (GameStateManager.Instance.GameState == GameStates.IdleState)
        {
            if (MouseManager.Instance.MouseoveredUnit != null)
            {
                SetCursorTo(clickingSelectionCursor, selectionCursor);
            }
            else
            {
                SetCursorTo(clickingDefaultCursor, defaultCursor);
            }
        }
    }

    private void CheckForAnimatingState()
    {
        if (GameStateManager.Instance.GameState == GameStates.AnimatingState)
        {
            Cursor.SetCursor(waitingCursor, GetHotspotType(waitingCursor), curMode);
            currentCursor = waitingCursor;
        }
    }


    private void SetCursorTo(Texture2D clickingCursor, Texture2D normalCursor)
    {
        if (Input.GetMouseButton(0))
        {
            Cursor.SetCursor(clickingCursor, GetHotspotType(clickingCursor), curMode);
            currentCursor = clickingCursor;
        }
        else
        {
            Cursor.SetCursor(normalCursor, GetHotspotType(normalCursor), curMode);
            currentCursor = normalCursor;
        }

    }

    private Vector2 GetHotspotType(Texture2D cursor)
    {
        if (MiddleHotspotCursors.Contains(cursor))
        {
            return middleHotspot;
        }
        else
        {
            return basicHotspot;
        }
    }

    private bool IsTheTileValidForEnterCombatCursor()
    {
        //we actually CAN assume there IS a selected unit.
        return MouseManager.Instance.mouseoveredTile.IsProtectedByEnemyOf(MouseManager.Instance.SelectedUnit);
    }    

    public void SetCursorToInfo()
    {
        skipSettingCursor = true;
        previousCursor = currentCursor;
        Cursor.SetCursor(infoCursor, basicHotspot, curMode);
        currentCursor = infoCursor;

    }

    public void StopSettingInfoCursor()
    {
        skipSettingCursor = false;
        currentCursor = previousCursor;
        Cursor.SetCursor(currentCursor, GetHotspotType(currentCursor), curMode);
    }

    private void CheckWhatUnitAreWeOver(Texture2D clickingAlly, Texture2D ally, Texture2D clickingEnemy, Texture2D enemy)
    {
        if (MouseManager.Instance.MouseoveredUnit.owner == Global.instance.playerTeams[TurnManager.Instance.PlayerToMove].players[0])
        {
            SetCursorTo(clickingAlly, ally);
        }
        else
        {
            SetCursorTo(clickingEnemy, enemy);
        }
    }

    void CheckWhatTileAreWeOver()
    {
        if (MouseManager.Instance.SelectedUnit.IsInCombat())
        {
            if (IsTheTileValidForEnterCombatCursor())
            {
                SetCursorTo(clickingBlockedCursor, blockedCursor);
            }
            else if (MouseManager.Instance.mouseoveredTile.neighbours.Contains(MouseManager.Instance.SelectedUnit.currentPosition))
            {
                SetCursorTo(clickingQCMovementCursor, QCMovementCursor);
            }
            else
            {
                SetCursorTo(clickingBlockedCursor, blockedCursor);
            }
        }
        else
        {            
            if (Pathfinder.instance.IsLegalTileForUnit(MouseManager.Instance.mouseoveredTile, MouseManager.Instance.SelectedUnit))
            {                
                if (IsTheTileValidForEnterCombatCursor())
                {
                    SetCursorTo(clickingEnterCombatCursor, enterCombatCursor);
                }
                else
                {
                    SetCursorTo(clickingWalkingCursor, walkingCursor);
                }
            }
            else
            {
                SetCursorTo(clickingBlockedCursor, blockedCursor);
            }
        }

        /* if (MovementController.Instance.temporaryPositions != null && MovementController.Instance.temporaryPositions.Count > 0 && MovementController.Instance.temporaryPositions[MovementController.Instance.temporaryPositions.Count - 1].gameObject == MouseManager.Instance.mouseoveredTile)
         {
             SetCursorTo(clickingTargetCursor, targetCursor);
         }*/
    }
}
