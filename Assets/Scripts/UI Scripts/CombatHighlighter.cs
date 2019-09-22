using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatHighlighter : MonoBehaviour
{
    [SerializeField] Color myColor;

    void Update()
    {
        ToggleHighlight();
        if (Input.GetKeyUp(KeyCode.LeftControl) && TurnManager.Instance.CurrentPhase == TurnPhases.Shooting)
        {
            foreach (Tile tile in Map.Board)
            {
                if (tile.GetComponent<Renderer>().material.color == myColor && tile.isBeingColoredByCombatHighlighter == true)
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

        if (TurnManager.Instance.CurrentPhase == TurnPhases.Attack && MouseManager.Instance.SelectedUnit != null && MouseManager.Instance.SelectedUnit.CheckIfIsInCombat() == true && MouseManager.Instance.SelectedUnit.hasAttacked == false && MouseManager.Instance.SelectedUnit.CanAttack)
        {
            foreach (Tile tile in Map.Board)
            {
                if (tile.myUnit != null && tile.myUnit.PlayerID != TurnManager.Instance.PlayerHavingTurn && tile.myUnit.EnemyList.Contains(MouseManager.Instance.SelectedUnit) == true)
                {
                    tile.GetComponent<Renderer>().material.color = Color.red;
                    tile.isBeingColoredByCombatHighlighter = true;
                }
            }
            return;
        }


        if (TurnManager.Instance.CurrentPhase == TurnPhases.Shooting && MouseManager.Instance.SelectedUnit != null && MouseManager.Instance.SelectedUnit.isRanged == true && MouseManager.Instance.SelectedUnit.GetComponent<ShootingScript>().CanShoot == true)
        {
            foreach (Tile tile in Map.Board)
            {
                if (tile.myUnit != null && tile.myUnit.PlayerID != TurnManager.Instance.PlayerHavingTurn && ShootingScript.WouldItBePossibleToShoot(MouseManager.Instance.SelectedUnit.GetComponent<ShootingScript>(), MouseManager.Instance.SelectedUnit.transform.position, tile.transform.position).Key)
                {
                    tile.GetComponent<Renderer>().material.color = Color.red;
                    tile.isBeingColoredByCombatHighlighter = true;
                }
                else if (tile.myUnit == null && Input.GetKey(KeyCode.LeftControl) && ShootingScript.WouldItBePossibleToShoot(MouseManager.Instance.SelectedUnit.GetComponent<ShootingScript>(), MouseManager.Instance.SelectedUnit.transform.position, tile.transform.position).Key)
                {
                    tile.GetComponent<Renderer>().material.color = myColor;
                    tile.isBeingColoredByCombatHighlighter = true;
                }
            }
            return;
        }


        foreach (Tile tile in Map.Board)
        {
            if (tile.isBeingColoredByCombatHighlighter)
            {
                tile.isBeingColoredByCombatHighlighter = false;
                tile.GetComponent<Renderer>().material.color = Color.white;
            }
        }


    }

}
