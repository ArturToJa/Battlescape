using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BattlescapeLogic;

public class UnitHighlighter : MonoBehaviour
{    
    static List<Tile> ControlledTiles = new List<Tile>();

    public static void ToggleHighlight()
    {
        bool thereIsAUnitWhoCanMove = false;

        if (Input.GetKey(KeyCode.LeftControl) == false)
        {
            foreach (Tile tile in Map.Board)
            {
                if (ControlledTiles.Contains(tile))
                {
                    ControlledTiles.Remove(tile);
                    BattlescapeGraphics.ColouringTool.SetColour(tile, Color.white);
                }
            }
        }
        else
        {            
            if (TurnManager.Instance.CurrentPhase == TurnPhases.Movement && (GameStateManager.Instance.GameState == GameStates.IdleState ||GameStateManager.Instance.GameState == GameStates.MoveState))
            {
                foreach (Tile tile in Map.Board)
                {
                    if (tile.myUnit != null && tile.myUnit.owner == Global.instance.playerTeams[TurnManager.Instance.PlayerToMove].players[0] && tile.myUnit.CanStillMove())
                    {
                        BattlescapeGraphics.ColouringTool.SetColour(tile, Color.green);
                        if (ControlledTiles.Contains(tile) == false)
                        {
                            ControlledTiles.Add(tile);
                        }
                        thereIsAUnitWhoCanMove = true;
                    }
                }
                if (thereIsAUnitWhoCanMove == false && TurnManager.Instance.TurnCount > 0)
                {
                    PopupTextController.AddPopupText("No more units can move!", PopupTypes.Info);
                }
            }            
            else if (TurnManager.Instance.CurrentPhase == TurnPhases.Attack && (GameStateManager.Instance.GameState == GameStates.IdleState || GameStateManager.Instance.GameState == GameStates.AttackState))
            {
                foreach (Tile tile in Map.Board)
                {
                    if 
                        (tile.myUnit != null 
                        && tile.myUnit.owner == Global.instance.playerTeams[TurnManager.Instance.PlayerToMove].players[0]
                        && tile.myUnit.CanStillAttack() == true 
                        && tile.myUnit.attack != null)
                    {
                        BattlescapeGraphics.ColouringTool.SetColour(tile, Color.green);
                        ControlledTiles.Add(tile);
                        thereIsAUnitWhoCanMove = true;
                    }
                }
                if (thereIsAUnitWhoCanMove == false && TurnManager.Instance.TurnCount > 0)
                {
                    PopupTextController.AddPopupText("No more units can attack!", PopupTypes.Info);
                }
            }            
        }
        
    }

}
