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
        foreach (Unit unit in GameRound.instance.currentPlayer.playerUnits)
        {
            Tile newTile = ChooseTheTile();
            DropZone.Instance.SendCommandToSetUnitPosition(unit, newTile);
        }
    }

    public void PositionUnits()
    {
        Position(SaveLoadManager.instance.playerArmy.heroIndex, GameRound.instance.currentPlayer.team.index, ChooseTheTile());
        foreach (int index in SaveLoadManager.instance.playerArmy.unitIndecies)
        {
            Position(index, GameRound.instance.currentPlayer.team.index, ChooseTheTile());
        }
    }

    public void Position(int index, int playerID, Tile tile)
    {

        if (tile == null)
        {
            return;
        }
        DropZone.Instance.CommandInstantiateUnit(index, playerID, tile.transform.position);
    }


    public Tile ChooseTheTile()
    {
        List<Tile> possibleTiles = new List<Tile>();
        foreach (Tile tile in Global.instance.currentMap.board)
        {
            if (tile.IsWalkable() && (tile.dropzoneOfTeam == GameRound.instance.currentPlayer.team.index))
            {
                possibleTiles.Add(tile);
            }
        }

        Tile chosenTile = ChooseRandomTileFromList(possibleTiles);

        if (chosenTile == null)
        {
            Debug.LogError("Too many units, couldn't put them on battlefield");
            return null;
        }
        return chosenTile;
    }

    static Tile ChooseRandomTileFromList(List<Tile> tileList)
    {
        Tile chosenTile = null;
        while (tileList.Count > 0)
        {
            chosenTile = tileList[Random.Range(0, tileList.Count)];
            if (chosenTile.IsWalkable() == false || chosenTile.hasObstacle)
            {
                tileList.Remove(chosenTile);
            }
            else
            {
                break;
            }
        }

        return chosenTile;
    }
}
