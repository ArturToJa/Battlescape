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
            MultiTile newPosition = ChooseThePosition(unit.currentPosition.size);
            DropZone.instance.SendCommandToSetUnitPosition(unit, newPosition);
        }
    }

    public void CreateUnits()
    {
        DropZone.instance.CommandInstantiateUnit(Global.instance.armySavingManager.currentSave.heroPrefabPath, GameRound.instance.currentPlayer.team.index);
        foreach (string index in Global.instance.armySavingManager.currentSave.unitPrefabPaths)
        {
            DropZone.instance.CommandInstantiateUnit(index, GameRound.instance.currentPlayer.team.index);
        }
    }   


    public MultiTile ChooseThePosition(Size size)
    {
        List<MultiTile> possibleTiles = new List<MultiTile>();
        foreach (Tile tile in Global.instance.currentMap.board)
        {
            MultiTile position = MultiTile.Create(tile, size);
            if (position != null && position.IsWalkable() && (position.IsDropzoneOfTeam(GameRound.instance.currentPlayer.team.index)))
            {
                possibleTiles.Add(position);
            }
        }

        MultiTile chosenTile = Tools.GetRandomElementFromList<MultiTile>(possibleTiles);

        if (chosenTile == null)
        {
            Debug.LogError("Too many units, couldn't put them on battlefield");
            return null;
        }
        return chosenTile;
    }

      
}
