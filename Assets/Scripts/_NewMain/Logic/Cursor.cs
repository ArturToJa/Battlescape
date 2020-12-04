using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.EventSystems;


namespace BattlescapeLogic
{
    public class Cursor : MonoBehaviour
    {
        public static Cursor instance { get; private set; }

        [SerializeField] List<Texture2D> middleHotspotCursors;

        [SerializeField] Texture2D defaultCursor;
        [SerializeField] Texture2D clickingDefaultCursor;
        [SerializeField] Texture2D waitingCursor;
        [SerializeField] Texture2D shootingCursor;
        [SerializeField] Texture2D clickingShootingCursor;
        [SerializeField] Texture2D attackCursor;
        [SerializeField] Texture2D clickingAttackCursor;
        [SerializeField] Texture2D walkingCursor;
        [SerializeField] Texture2D clickingWalkingCursor;
        [SerializeField] Texture2D validTargetCursor;
        [SerializeField] Texture2D clickingTargetCursor;
        [SerializeField] Texture2D infoCursor;
        [SerializeField] Texture2D selectionCursor;
        [SerializeField] Texture2D clickingSelectionCursor;
        [SerializeField] Texture2D blockedCursor;
        [SerializeField] Texture2D clickingBlockedCursor;
        [SerializeField] Texture2D enterCombatCursor;
        [SerializeField] Texture2D clickingEnterCombatCursor;
        [SerializeField] Texture2D combatExitingMovementCursor;
        [SerializeField] Texture2D clickingCombatExitingMovementCursor;

        Texture2D currentBasicCursor;
        Texture2D currentClickingCursor;

        Vector2 basicHotspot = Vector2.zero;
        Vector2 middleHotspot = new Vector2(32, 32);
        public bool isInfoByUI { get; set; }

        private void Start()
        {
            isInfoByUI = false;
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(this.gameObject);
            }
        }

        void Update()
        {
            if (PlayerInput.instance.isInputBlocked)
            {
                UnityEngine.Cursor.SetCursor(waitingCursor, GetHotspotType(waitingCursor), CursorMode.Auto);
                return;
            }

            if (Input.GetMouseButton(0))
            {
                UnityEngine.Cursor.SetCursor(currentClickingCursor, GetHotspotType(currentClickingCursor), CursorMode.Auto);
                return;
            }
            else
            {
                UnityEngine.Cursor.SetCursor(currentBasicCursor, GetHotspotType(currentBasicCursor), CursorMode.Auto);
                return;
            }
        }

        void SetCursorTo(Texture2D basicCursor, Texture2D clickingCursor)
        {
            currentBasicCursor = basicCursor;
            currentClickingCursor = clickingCursor;
        }

        Vector2 GetHotspotType(Texture2D cursor)
        {
            if (middleHotspotCursors.Contains(cursor))
            {
                return middleHotspot;
            }
            else
            {
                return basicHotspot;
            }
        }

        public void OnLegalTargetHovered()
        {
            SetCursorTo(validTargetCursor, clickingTargetCursor);
        }

        public void OnInvalidTargetHovered()
        {
            SetCursorTo(blockedCursor, blockedCursor);
        }

        public void OnSelectableHovered()
        {
            SetCursorTo(selectionCursor, clickingSelectionCursor);
        }

        public void OnEnemyHovered(Unit selected, IDamageable enemy)
        {
            if (selected.attack == null)
            {
                OnInvalidTargetHovered();
                return;
            }            
            if (selected.attack is MeleeAttack)
            {
                SetCursorTo(attackCursor, clickingAttackCursor);
                return;
            }
            if (selected.attack is ShootingAttack)
            {
                SetCursorTo(shootingCursor, clickingShootingCursor);
                return;
            }
        }

        public void ShowInfoCursor()
        {
            SetCursorTo(infoCursor, infoCursor);
        }

        public void OnTileToMoveHovered(Unit unitToMove, MultiTile targetTile)
        {
            if (targetTile.IsProtectedByEnemyOf(unitToMove))
            {
                SetCursorTo(enterCombatCursor, clickingEnterCombatCursor);
            }
            else if (unitToMove.IsInCombat())
            {
                SetCursorTo(combatExitingMovementCursor, clickingCombatExitingMovementCursor);
            }
            else
            {
                SetCursorTo(walkingCursor, clickingWalkingCursor);
            }
        }

        public void SetToDefault()
        {
            SetCursorTo(defaultCursor, clickingDefaultCursor);
        }
    }
}


