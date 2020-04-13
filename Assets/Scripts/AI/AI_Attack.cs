using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BattlescapeLogic;

public class AI_Attack : AI_Base_Attack
{

    public AI_Attack(int ID) : base(ID)
    {
        base.CallTheConstructor(ID);
    }

    protected override Queue<BattlescapeLogic.Unit> GetPossibleUnits()
    {
        Queue<BattlescapeLogic.Unit> myUnitsToMove = new Queue<BattlescapeLogic.Unit>(allyList);
        BattlescapeLogic.Unit firstUnit = myUnitsToMove.Peek();
        while (firstUnit.IsInCombat() == false || firstUnit.CanStillAttack() == false)
        {
            myUnitsToMove.Dequeue();
            if (myUnitsToMove.Count == 0)
            {
                AI_Controller.Instance.ClearAIs();
                //GameStateManager.NextPhase();
                return null;
            }
            else
            {
                firstUnit = myUnitsToMove.Peek();
            }
        }
        return myUnitsToMove;
    }

    protected override void PerformTheAction(BattlescapeLogic.Unit currentUnit, KeyValuePair<Tile, float> target)
    {
        Tile theTile = target.Key;
        AI_Controller.tilesAreEvaluated = false;
        if (theTile != null)
        {
            Unit attackTarget = theTile.myUnit;
            currentUnit.statistics.numberOfAttacks = 0;
            NetworkingApiBaseClass.Instance.SendCommandToStartAttack(currentUnit, attackTarget);
        }
        else
        {
            currentUnit.statistics.numberOfAttacks = 0;
        }
        
    }

    public override List<Tile> GetPossibleMoves(BattlescapeLogic.Unit currentUnit, bool isAlly)
    {
        List<Tile> enemiesInCombat = new List<Tile>();
        foreach (Tile tile in currentUnit.currentPosition.neighbours)
        {
            if (tile.myUnit != null && tile.myUnit.GetMyOwner() != currentUnit.GetMyOwner())
            {
                enemiesInCombat.Add(tile);
            }
        }
        return enemiesInCombat;
    }
    public override float EvaluateAsATarget(BattlescapeLogic.Unit currentUnit, Tile startingTile, Tile enemyTile)
    {
        // here our unit makes a decision of where to attack at. He will:
        // 1. Want to prio units who cannot counterattack
        // 2. Want to hit guys, who will give him more points
        // 3. Want to hit guys, who he has bigger chance to actually damage.

        BattlescapeLogic.Unit target = enemyTile.myUnit;
        float Evaluation = 0f;
        //        Debug.Log("Evaluating a shot by: " + currentUnit + " at: " + target);
        // 1.
        if (target.statistics.numberOfRetaliations == 0)
        {
            Evaluation += 0.2f;
        }

        // 2.
        Evaluation += target.statistics.cost * 0.1f;
        if (target is Hero)
        {
            Evaluation += 0.75f;
            // Thats just my ruff estimate of "value" of a hero here, lol.
        }
        // Debug.Log("Evaluation increased by: " + target.Value * 0.1f + " because of arget's value");
        // NOTE - this might want to get changed to "AI Value" when we add AIData scripts for units.

        // 3.
        float temp = ChancesToHit(currentUnit, target);
        Evaluation += temp;
        // Debug.Log("Evaluation increased by: " + temp + " for chances to hit");
        return Evaluation;
    }
    float ChancesToHit(BattlescapeLogic.Unit currentUnit,BattlescapeLogic.Unit target)
    {
        float EvaluationIncrease = 0f;
        float damageChance = 100; // - DamageCalculator.MissChance(currentUnit, target);
        // im NOT sure if line above is correct, i had to change it after changing the combat system, it might be crap.
        float temp = damageChance * 0.01f;
        EvaluationIncrease += temp;
        return EvaluationIncrease;
    }
}
