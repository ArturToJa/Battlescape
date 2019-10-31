using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BattlescapeLogic;

public class PreGameAI
{
    // this class is supposed to make an army "randomly" position itself in the begining of a fight. I want it also to be an option for PLAYER's army - so that a "casual" player, who does not want to 
    // have to think about it - can also click it and not think about positioning army before the battle.


    public void RepositionUnits()
    {
        foreach (UnitScript unit in Global.instance.playerTeams[TurnManager.Instance.PlayerToMove].Players[0].playerUnits)
        {
            Tile newTile = ChooseTheTile();
            DragableUnit.SetNewPosition(unit.myTile.position.x, unit.myTile.position.z, newTile.position.x, newTile.position.z);
        }
    }

    public void PositionUnits()
    {
        int heroID = (int)SaveLoadManager.Instance.playerArmy.heroID;
        Position(heroID, Global.instance.playerTeams[TurnManager.Instance.PlayerToMove].Players[0].team.index, ChooseTheTile());
        foreach (UnitID integer in SaveLoadManager.Instance.playerArmy.unitIDs)
        {
            Position((int)integer, Global.instance.playerTeams[TurnManager.Instance.PlayerToMove].Players[0].team.index, ChooseTheTile());
        }
        /*foreach (Transform unit in DeploymentPanel)
        {
            
                    
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
        }*/
    }

    public void Position(int unitID, int playerID, Tile tile)
    {

        if (tile == null)
        {
            return;
        }
        DropZone.Instance.CommandInstantiateUnit(unitID, playerID, tile.transform.position);
    }


    public Tile ChooseTheTile()
    {
        List<Tile> possibleTiles = new List<Tile>();
        //List<Tile> greatTiles = new List<Tile>();
        foreach (Tile tile in Map.Board)
        {
            if (tile.IsWalkable() && ((tile.isDropzoneOfPlayer[Global.instance.playerTeams[TurnManager.Instance.PlayerToMove].Players[0].team.index])))
            {
                possibleTiles.Add(tile);
                /*if (me.thisUnitFirstPlayer.GetComponent<UnitScript>().isRanged && (tile.transform.position.x == 0 || tile.transform.position.x == 1 || tile.transform.position.x == 14 || tile.transform.position.x == 15) && tile.transform.position.z != 0 && tile.transform.position.z != 9)
                {
                    greatTiles.Add(tile);
                }
                else if (me.thisUnitFirstPlayer.GetComponent<UnitScript>().isRanged == false && (tile.transform.position.x == 1 || tile.transform.position.x == 2 || tile.transform.position.x == 13 || tile.transform.position.x == 14) && tile.transform.position.z != 0 && tile.transform.position.z != 9)
                {
                    greatTiles.Add(tile);
                }*/
            }
        }
        Tile chosenTile = null;
        //chosenTile = ChooseRandomTileFromList(greatTiles);
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
