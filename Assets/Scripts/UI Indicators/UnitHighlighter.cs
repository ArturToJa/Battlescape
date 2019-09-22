using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitHighlighter : MonoBehaviour
{
    bool toggle;
    bool isPressingControl = false;

    void Update()
    {
        if (Input.GetKey(KeyCode.LeftControl))
        {
            toggle = true;
            ToggleHighlight();
        }
        else if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            toggle = false;
            ToggleHighlight();
            isPressingControl = false;

        }
    }
    public void SwitchToggle()
    {
        toggle = !toggle;
    }

    public void ToggleHighlight()
    {
        bool thereIsAUnitWhoCanMove = false;

        if (toggle == false)
        {
            foreach (Tile tile in Map.Board)
            {
                if (tile.isBeingColoredByNormalHighlighter)
                {
                    tile.isBeingColoredByNormalHighlighter = false;
                    tile.GetComponent<Renderer>().material.color = Color.white;
                }
            }
        }
        else
        {
            if (TurnManager.Instance.CurrentPhase == TurnPhases.Movement && (GameStateManager.Instance.GameState == GameStates.IdleState ||GameStateManager.Instance.GameState == GameStates.MoveState))
            {
                foreach (Tile tile in Map.Board)
                {
                    if (tile.myUnit != null && tile.myUnit.PlayerID == TurnManager.Instance.PlayerToMove && MovementQuestions.Instance.CanUnitMoveAtAll(tile.myUnit.GetComponent<UnitMovement>()) == true)
                    {
                        tile.GetComponent<Renderer>().material.color = Color.green;
                        tile.isBeingColoredByNormalHighlighter = true;
                        thereIsAUnitWhoCanMove = true;
                    }
                }
                if (thereIsAUnitWhoCanMove == false && isPressingControl == false && TurnManager.Instance.TurnCount > 0)
                {
                    PopupTextController.AddPopupText("No more units can move!", PopupTypes.Info);
                }
            }

            if (TurnManager.Instance.CurrentPhase == TurnPhases.Shooting && (GameStateManager.Instance.GameState == GameStates.IdleState || GameStateManager.Instance.GameState == GameStates.ShootingState))
            {
                foreach (Tile tile in Map.Board)
                {
                    if (tile.myUnit != null && tile.myUnit.PlayerID == TurnManager.Instance.PlayerToMove && tile.myUnit.isRanged && tile.myUnit.GetComponent<ShootingScript>().CanShoot == true)
                    {
                        tile.GetComponent<Renderer>().material.color = Color.green;
                        tile.isBeingColoredByNormalHighlighter = true;
                        thereIsAUnitWhoCanMove = true;
                    }
                }
                if (thereIsAUnitWhoCanMove == false && isPressingControl == false && TurnManager.Instance.TurnCount > 0)
                {
                    PopupTextController.AddPopupText("No more units can shoot!", PopupTypes.Info);
                }
            }
            else if (TurnManager.Instance.CurrentPhase == TurnPhases.Attack && (GameStateManager.Instance.GameState == GameStates.IdleState || GameStateManager.Instance.GameState == GameStates.AttackState))
            {
                foreach (Tile tile in Map.Board)
                {
                    if (tile.myUnit != null && tile.myUnit.PlayerID == TurnManager.Instance.PlayerToMove && tile.myUnit.hasAttacked != true && tile.myUnit.CanAttack && tile.myUnit.CheckIfIsInCombat() == true)
                    {
                        tile.GetComponent<Renderer>().material.color = Color.green;
                        tile.isBeingColoredByNormalHighlighter = true;
                        thereIsAUnitWhoCanMove = true;
                    }
                }
                if (thereIsAUnitWhoCanMove == false && isPressingControl == false && TurnManager.Instance.TurnCount > 0)
                {
                    PopupTextController.AddPopupText("No more units can attack!", PopupTypes.Info);
                }
            }
            isPressingControl = true;
        }
        
    }

}
