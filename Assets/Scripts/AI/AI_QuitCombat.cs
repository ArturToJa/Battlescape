using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_QuitCombat : AI_Base_Movement
{
    static List<UnitScript> unitsSkippingTurn;

    public AI_QuitCombat(int ID) : base(ID)
    {
        base.CallTheConstructor(ID);
        if (unitsSkippingTurn == null)
        {
            unitsSkippingTurn = new List<UnitScript>();
        }
    }

    protected override Queue<UnitScript> GetPossibleUnits()
    {
        Queue<UnitScript> myUnitsToMove = new Queue<UnitScript>(allyList);
        UnitScript firstUnit = myUnitsToMove.Peek();
        while (firstUnit.CheckIfIsInCombat() == false || unitsSkippingTurn.Contains(firstUnit) == true || firstUnit.GetComponent<UnitMovement>().CanMove == false || firstUnit.hasJustArrivedToCombat || HasAnyLegalTiles(firstUnit) == false)
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

    bool HasAnyLegalTiles(UnitScript unit)
    {
        Pathfinder.Instance.BFS(unit.myTile, true);
        return Pathfinder.Instance.GetAllLegalTilesAndIfTheyAreSafe(unit.GetComponent<UnitMovement>(), true).Keys.Count > 0;
    }

    public override float EvaluateTile(UnitScript currentUnit, Tile tile, int enemiesDirection, Dictionary<UnitMovement, List<Tile>> EnemyMovementRange)
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

    private bool DoesUnitWantToQC(UnitScript unit)
    {
        // ok so when does a unit want to QC?
        // 1. if unit is a shooter - always (?)
        if (unit.isRanged)
        {
            return true;
        }
        // 2. if a unit is a hero - if there are too strong opponents in fight with him (2 really big guys)
        if (unit.GetComponent<HeroScript>() && unit.EnemyList.Count>1)
        {
            int bigGuys = 0;
            foreach (UnitScript enemy in unit.EnemyList)
            {
                if (enemy.isRanged == false && (enemy.CurrentAttack>unit.CurrentDefence || enemy.CurrentHP >= unit.CurrentHP + 2))
                {
                    bigGuys++;
                }
            }
            if (bigGuys >= 2)
            {
                return true;
            }
        }

        // 3. if a unit could reach shooters after that!
        Pathfinder.Instance.BFS(unit.myTile, true);
        List<Tile> temp = Pathfinder.Instance.GetAllTilesThatWouldBeLegalIfNotInCombat(unit, unit.GetComponent<UnitMovement>().GetCurrentMoveSpeed(true));
        foreach (UnitScript enemy in enemyList)
        {
            if (enemy.isRanged)
            {
                foreach (Tile tile in enemy.myTile.GetNeighbours())
                {
                    if (temp.Contains(tile))
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }

    protected override void PerformTheAction(UnitScript currentUnit, KeyValuePair<Tile, float> target)
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
