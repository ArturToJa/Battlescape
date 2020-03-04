using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BattlescapeLogic;
using UnityEngine.EventSystems;

public class CursorController : MonoBehaviour
{
    public static CursorController instance { get; private set; }

    [SerializeField] List<Texture2D> MiddleHotspotCursors;

    public Texture2D defaultCursor;
    public Texture2D clickingDefaultCursor;
    public Texture2D waitingCursor;
    public Texture2D shootingCursor;
    public Texture2D clickingShootingCursor;
    public Texture2D badRangeShootingCursor;
    public Texture2D clickingBadRangeShootingCursor;
    public Texture2D attackCursor;
    public Texture2D clickingAttackCursor;
    public Texture2D walkingCursor;
    public Texture2D clickingWalkingCursor;
    public Texture2D invalidTargetCursor;
    public Texture2D validTargetCursor;
    public Texture2D clickingTargetCursor;
    public Texture2D infoCursor;
    public Texture2D selectionCursor;
    public Texture2D clickingSelectionCursor;
    public Texture2D blockedCursor;
    public Texture2D clickingBlockedCursor;
    public Texture2D enterCombatCursor;
    public Texture2D clickingEnterCombatCursor;
    public Texture2D QCMovementCursor;
    public Texture2D clickingQCMovementCursor;

    Vector2 basicHotspot = Vector2.zero;
    Vector2 middleHotspot = new Vector2(32, 32);

    public bool isInfoByUI = false;

    private void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void SetCursorTo(Texture2D basicCursor, Texture2D clickingCursor)
    {

        if (PlayerInput.instance.isInputBlocked)
        {
            Cursor.SetCursor(waitingCursor, GetHotspotType(waitingCursor), CursorMode.Auto);
            return;
        }

        if (Input.GetMouseButton(0))
        {
            Cursor.SetCursor(clickingCursor, GetHotspotType(clickingCursor), CursorMode.Auto);
            return;
        }
        else
        {
            Cursor.SetCursor(basicCursor, GetHotspotType(basicCursor), CursorMode.Auto);
            return;
        }
    }   

    public void OnUnitHovered(Unit unit)
    {
        if (AbstractActiveAbility.currentlyUsedAbility != null)
        {
            if (AbstractActiveAbility.currentlyUsedAbility.IsLegalTarget(unit))
            {
                SetCursorTo(validTargetCursor, clickingTargetCursor);
                return;
            }
            else
            {
                SetCursorTo(invalidTargetCursor, invalidTargetCursor);
                return;
            }
        }
        if (MouseManager.instance.CanSelect(unit))
        {
            SetCursorTo(selectionCursor, clickingSelectionCursor);
        }
        else if (MouseManager.instance.IsLegalToDeclareAttack(MouseManager.instance.selectedUnit, unit))
        {
            if (unit.attack == null)
            {
                SetCursorTo(blockedCursor, clickingBlockedCursor);
            }
            else if (unit.attack is MeleeAttack)
            {
                SetCursorTo(attackCursor, clickingAttackCursor);
            }
            else
            {
                SetCursorTo(shootingCursor, clickingShootingCursor);
            }
        }
        else
        {
            SetCursorTo(infoCursor, infoCursor);
        }
    }

    public void OnTileHovered(Tile tile)
    {
        if (AbstractActiveAbility.currentlyUsedAbility != null)
        {
            if (AbstractActiveAbility.currentlyUsedAbility.IsLegalTarget(tile))
            {
                SetCursorTo(validTargetCursor, clickingTargetCursor);
                return;
            }
            else
            {
                SetCursorTo(invalidTargetCursor, invalidTargetCursor);
                return;
            }
        }
        if (MouseManager.instance.selectedUnit == null)
        {
            SetCursorTo(defaultCursor, clickingDefaultCursor);
        }
        else if (MouseManager.instance.CanSelectedUnitMoveTo(tile))
        {
            if (tile.IsProtectedByEnemyOf(MouseManager.instance.selectedUnit))
            {
                SetCursorTo(enterCombatCursor, clickingEnterCombatCursor);
            }
            else if (MouseManager.instance.selectedUnit.IsInCombat())
            {
                SetCursorTo(QCMovementCursor, clickingQCMovementCursor);
            }
            else
            {
                SetCursorTo(walkingCursor, clickingWalkingCursor);
            }
        }
        else
        {
            SetCursorTo(blockedCursor, clickingBlockedCursor);
        }
    }

    Vector2 GetHotspotType(Texture2D cursor)
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


}
