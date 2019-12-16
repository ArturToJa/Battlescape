using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BattlescapeLogic;

public class CombatHighlighter : MonoBehaviour
{
    [SerializeField] Color myColor;
    List<Tile> ControlledTiles = new List<Tile>();

    void Update()
    {
        ToggleHighlight();
        if (Input.GetKeyUp(KeyCode.LeftControl) && TurnManager.Instance.CurrentPhase == TurnPhases.Attack)
        {
            foreach (Tile tile in Map.Board)
            {
                if (tile.GetComponent<Renderer>().material.color == myColor && ControlledTiles.Contains(tile))
                {
                    tile.GetComponent<Renderer>().material.color = Color.white;
                }
            }
        }
    }

    public void ToggleHighlight()
    {
        if (GameStateManager.Instance.IsCurrentPlayerAI() || GameStateManager.Instance.GameState == GameStates.TargettingState)
        {
            return;
        }

        if (TurnManager.Instance.CurrentPhase == TurnPhases.Attack && MouseManager.Instance.SelectedUnit != null && MouseManager.Instance.SelectedUnit.CanStillAttack() == true)
        {
            foreach (Tile tile in Map.Board)
            {
                if (tile.myUnit != null && tile.myUnit.owner.team != Global.instance.playerTeams[TurnManager.Instance.PlayerHavingTurn] && (tile.myUnit.currentPosition.neighbours.Contains(MouseManager.Instance.SelectedUnit.currentPosition) == true || CombatController.Instance.WouldItBePossibleToShoot(MouseManager.Instance.SelectedUnit, MouseManager.Instance.SelectedUnit.transform.position, tile.transform.position)))
                {
                    tile.GetComponent<Renderer>().material.color = Color.red;
                    ControlledTiles.Add(tile);
                }
            }
            return;
        }       

        foreach (Tile tile in Map.Board)
        {
            if (ControlledTiles.Contains(tile))
            {
                ControlledTiles.Remove(tile);
                tile.GetComponent<Renderer>().material.color = Color.white;
            }
        }


    }

}
