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
        if (Input.GetKeyUp(KeyCode.LeftControl) && TurnManager.Instance.CurrentPhase == TurnPhases.Shooting)
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

        if (TurnManager.Instance.CurrentPhase == TurnPhases.Attack && MouseManager.Instance.SelectedUnit != null && MouseManager.Instance.SelectedUnit.CheckIfIsInCombat() == true && MouseManager.Instance.SelectedUnit.CanStillAttack() == true && MouseManager.Instance.SelectedUnit.CanAttack)
        {
            foreach (Tile tile in Map.Board)
            {
                if (tile.myUnit != null && tile.myUnit.PlayerID != Global.instance.playerTeams[TurnManager.Instance.PlayerHavingTurn].players[0].team.index && tile.myUnit.EnemyList.Contains(MouseManager.Instance.SelectedUnit) == true)
                {
                    tile.GetComponent<Renderer>().material.color = Color.red;
                    ControlledTiles.Add(tile);
                }
            }
            return;
        }


        if (TurnManager.Instance.CurrentPhase == TurnPhases.Shooting && MouseManager.Instance.SelectedUnit != null && MouseManager.Instance.SelectedUnit.isRanged == true && MouseManager.Instance.SelectedUnit.GetComponent<ShootingScript>().CanShoot == true)
        {
            foreach (Tile tile in Map.Board)
            {
                if (tile.myUnit != null && tile.myUnit.PlayerID != Global.instance.playerTeams[TurnManager.Instance.PlayerHavingTurn].players[0].team.index && ShootingScript.WouldItBePossibleToShoot(MouseManager.Instance.SelectedUnit, MouseManager.Instance.SelectedUnit.transform.position, tile.transform.position).Key)
                {
                    tile.GetComponent<Renderer>().material.color = Color.red;
                    ControlledTiles.Add(tile);
                }
                else if (tile.myUnit == null && Input.GetKey(KeyCode.LeftControl) && ShootingScript.WouldItBePossibleToShoot(MouseManager.Instance.SelectedUnit, MouseManager.Instance.SelectedUnit.transform.position, tile.transform.position).Key)
                {
                    tile.GetComponent<Renderer>().material.color = myColor;
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
