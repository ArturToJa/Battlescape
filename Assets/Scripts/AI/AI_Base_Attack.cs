using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BattlescapeLogic;

public class AI_Base_Attack : AI_BaseClass
{

    public AI_Base_Attack(int ID) : base(ID)
    {
        base.CallTheConstructor(ID);
    }

    public override IEnumerator EvaluatePossibleMoves(BattlescapeLogic.Unit currentUnit, List<Tile> possibleMoves)
    {
        AI_Controller.isEvaluatingTiles = true;
        EvaluatedTiles = new Dictionary<Tile, float>();

        foreach (var tile in possibleMoves)
        {
            EvaluatedTiles.Add(tile, EvaluateAsATarget(currentUnit, tile));

            yield return null;
        }
        AI_Controller.isEvaluatingTiles = false;
        AI_Controller.tilesAreEvaluated = true;
    }

    public virtual float EvaluateAsATarget(BattlescapeLogic.Unit currentUnit, Tile tile)
    {
        return EvaluateAsATarget(currentUnit, currentUnit.currentPosition,tile);
    }

    public virtual float EvaluateAsATarget(BattlescapeLogic.Unit currentUnit, Tile startingTile, Tile enemyTile)
    {
        Debug.LogError("Bad one got called?");
        //HERE we DONT want to do stuff!
        return -Mathf.Infinity;
    }

}
