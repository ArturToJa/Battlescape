using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BattlescapeLogic;

public class PreGameAI
{
    // this class is supposed to make an army "randomly" position itself in the begining of a fight. I want it also to be an option for PLAYER's army - so that a "casual" player, who does not want to 
    // have to think about it - can also click it and not think about positioning army before the battle.

    public PreGameAI(Transform DeploP)
    {
        DeploymentPanel = DeploP;
    }
    Transform DeploymentPanel;

    public void PositionUnits()
    {
        foreach (Transform unit in DeploymentPanel)
        {
            Position(unit.GetComponent<DragableUnitIcon>().me, ChooseTheTile(unit.GetComponent<DragableUnitIcon>().me));            
        }
        int tempInt = DeploymentPanel.transform.childCount;
        for (int i = 0; i < tempInt; i++)
        {
            if (Application.isEditor)
            {
                Object.DestroyImmediate(DeploymentPanel.transform.GetChild(0).gameObject);
            }
            else
            {
                Object.Destroy(DeploymentPanel.transform.GetChild(i).gameObject);
            }
        }
    }

    public void Position(Unit unit, Tile tile)
    {

        if (tile == null)
        {
            return;
        }

        DropZone.Instance.CommandInstantiateUnit((int)unit.myUnitID, TurnManager.Instance.PlayerToMove, tile.transform.position);
    }


    public Tile ChooseTheTile(Unit me)
    {
        List<Tile> possibleTiles = new List<Tile>();
        List<Tile> greatTiles = new List<Tile>();
        foreach (Tile tile in Map.Board)
        {
            if (tile.IsWalkable() && tile.hasObstacle == false && ((tile.isDropzoneOfPlayer[TurnManager.Instance.PlayerToMove])))
            {
                possibleTiles.Add(tile);
                if (me.thisUnitFirstPlayer.GetComponent<UnitScript>().isRanged && (tile.transform.position.x == 0 || tile.transform.position.x == 1 || tile.transform.position.x == 14 || tile.transform.position.x == 15) && tile.transform.position.z != 0 && tile.transform.position.z != 9)
                {
                    greatTiles.Add(tile);
                }
                else if (me.thisUnitFirstPlayer.GetComponent<UnitScript>().isRanged == false && (tile.transform.position.x == 1 || tile.transform.position.x == 2 || tile.transform.position.x == 13 || tile.transform.position.x == 14) && tile.transform.position.z != 0 && tile.transform.position.z != 9)
                {
                    greatTiles.Add(tile);
                }
            }
        }
        Tile chosenTile = null;
        chosenTile = ChooseRandomTileFromList(greatTiles);
        if (chosenTile == null)
        {
            chosenTile = ChooseRandomTileFromList(possibleTiles);
        }
        if (chosenTile == null)
        {
            Debug.LogError("Too many units, couldn't put them on battlefield");
            return null;
        }
      //  Debug.Log("tile" + chosenTile);
        return chosenTile;
    }

    static Tile ChooseRandomTileFromList(List<Tile> TileList)
    {
        Tile chosenTile = null;
        while (TileList.Count > 0)
        {
            chosenTile = TileList[Random.Range(0, TileList.Count)];
            if (chosenTile.IsWalkable() == false || chosenTile.hasObstacle)
            {
                TileList.Remove(chosenTile);
            }
            else
            {
                break;
            }
        }

        return chosenTile;
    }
}
