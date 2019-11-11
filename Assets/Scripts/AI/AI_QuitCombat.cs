using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BattlescapeLogic;

public class AI_QuitCombat : AI_Base_Movement
{
    static List<BattlescapeLogic.Unit> unitsSkippingTurn;

    public AI_QuitCombat(int ID) : base(ID)
    {
        base.CallTheConstructor(ID);
        if (unitsSkippingTurn == null)
        {
            unitsSkippingTurn = new List<BattlescapeLogic.Unit>();
        }
    }

    protected override Queue<BattlescapeLogic.Unit> GetPossibleUnits()
    {
        Queue<BattlescapeLogic.Unit> myUnitsToMove = new Queue<BattlescapeLogic.Unit>(allyList);
        BattlescapeLogic.Unit firstUnit = myUnitsToMove.Peek();
        while (firstUnit.IsInCombat() == false || unitsSkippingTurn.Contains(firstUnit) == true || firstUnit.statistics.movementPoints <= 0|| HasAnyLegalTiles(firstUnit) == false)
        {
            myUnitsToMove.Dequeue();
            if (myUnitsToMove.Count == 0)
            {
                AI_Controller.Instance.ClearAIs();
                AI_Controller.didAllTheQCDecisionsHappen = true;
                return null;
            }
            else
            {
                firstUnit = myUnitsToMove.Peek();
            }
        }
        return myUnitsToMove;
    }

    bool HasAnyLegalTiles(BattlescapeLogic.Unit unit)
    {
        return Pathfinder.instance.GetAllLegalTilesFor(unit).Count > 0;
    }

    public override float EvaluateTile(BattlescapeLogic.Unit currentUnit, Tile tile, int enemiesDirection, Dictionary<BattlescapeLogic.Unit, List<Tile>> EnemyMovementRange)
    {
        // we want to evaluate if a tile is a proper "end" tile for a unit to QC into
        float Evaluation = 0f;
        // first lets check if a unit WANTS to QC at all! If not lets give a -infinity
        if (DoesUnitWantToQC(currentUnit) == false)
        {
            Evaluation = -Mathf.Infinity;
        }
        else
        {
            // lets see if the tile is good for QC in a NORMAL way we would do it in movement!
            AI_Movement tempAI = new AI_Movement(ID);
            Evaluation = tempAI.EvaluateTile(currentUnit, tile, enemiesDirection, EnemyMovementRange);
        }
        return Evaluation;
    }

    private bool DoesUnitWantToQC(BattlescapeLogic.Unit unit)
    {
        // ok so when does a unit want to QC?
        // 1. if unit is a shooter - always (?)
        if (unit.IsRanged())
        {
            return true;
        }
        // 2. if a unit is a hero - if there are too strong opponents in fight with him (2 really big guys)
        //if (unit.GetComponent<HeroScript>() && unit.EnemyList.Count>1)
        //{
        //    int bigGuys = 0;
        //    foreach (BattlescapeLogic.Unit enemy in unit.EnemyList)
        //    {
        //        if (enemy.IsRanged() == false && (enemy.statistics.GetCurrentAttack()>unit.statistics.GetCurrentDefence() || enemy.statistics.healthPoints >= unit.statistics.healthPoints + 2))
        //        {
        //            bigGuys++;
        //        }
        //    }
        //    if (bigGuys >= 2)
        //    {
        //        return true;
        //    }
        //}

        // 3. if a unit could reach shooters after that!
        //List<Tile> temp = Pathfinder.instance.GetAllTilesThatWouldBeLegalIfNotInCombat(unit, unit.GetComponent<BattlescapeLogic.Unit>().GetCurrentMoveSpeed(true));
        //foreach (BattlescapeLogic.Unit enemy in enemyList)
        //{
        //    if (enemy.IsRanged())
        //    {
        //        foreach (Tile tile in enemy.currentPosition.neighbours)
        //        {
        //            if (temp.Contains(tile))
        //            {
        //                return true;
        //            }
        //        }
        //    }
        //}

        return false;
    }

    protected override void PerformTheAction(BattlescapeLogic.Unit currentUnit, KeyValuePair<Tile, float> target)
    {
        AI_Controller.tilesAreEvaluated = false;
        if (target.Key != null && target.Value > -Mathf.Infinity)
        {
            Debug.Log("Quitting Combat! Chosen tile is: " + target.Key);
            QCManager.Instance.QCForAI(target.Key);
            unitsSkippingTurn.Clear();
        }
        else
        {
            Debug.Log("No QC.");
            unitsSkippingTurn.Add(currentUnit);
        }
    }
}
