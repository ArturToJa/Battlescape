using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Attack : AI_Base_Attack
{

    public AI_Attack(int ID) : base(ID)
    {
        base.CallTheConstructor(ID);
    }

    protected override Queue<UnitScript> GetPossibleUnits()
    {
        Queue<UnitScript> myUnitsToMove = new Queue<UnitScript>(allyList);
        UnitScript firstUnit = myUnitsToMove.Peek();
        while (firstUnit.CheckIfIsInCombat() == false || firstUnit.hasAttacked == true)
        {
            myUnitsToMove.Dequeue();
            if (myUnitsToMove.Count == 0)
            {
                AI_Controller.Instance.ClearAIs();
                GameStateManager.NextPhase();
                return null;
            }
            else
            {
                firstUnit = myUnitsToMove.Peek();
            }
        }
        return myUnitsToMove;
    }

    protected override void PerformTheAction(UnitScript currentUnit, KeyValuePair<Tile, float> target)
    {
        Tile theTile = target.Key;
        AI_Controller.tilesAreEvaluated = false;
        if (theTile != null)
        {
            CombatController.Instance.AttackTarget = theTile.myUnit.GetComponent<UnitScript>();
            currentUnit.hasAttacked = true;
            CombatController.Instance.SendCommandToAttack(currentUnit, CombatController.Instance.AttackTarget, false, !currentUnit.isStoppingRetaliation);
        }
        else
        {
            currentUnit.hasAttacked = true;
        }
        
    }

    public override List<Tile> GetPossibleMoves(UnitScript currentUnit, bool isAlly)
    {
        List<Tile> enemiesInCombat = new List<Tile>();
        foreach (UnitScript enemy in currentUnit.EnemyList)
        {
            if (enemy.CurrentHP>0)
            {
                enemiesInCombat.Add(enemy.myTile);
            }
        }
        return enemiesInCombat;
    }
    public override float EvaluateAsATarget(UnitScript currentUnit, Tile startingTile, Tile enemyTile)
    {
        // here our unit makes a decision of where to attack at. He will:
        // 1. Want to prio units who cannot counterattack
        // 2. Want to hit guys, who will give him more points
        // 3. Want to hit guys, who he has bigger chance to actually damage.

        UnitScript target = enemyTile.myUnit;
        float Evaluation = 0f;
        //        Debug.Log("Evaluating a shot by: " + currentUnit + " at: " + target);
        // 1.
        if (target.CanCurrentlyRetaliate == false)
        {
            Evaluation += 0.2f;
        }

        // 2.
        Evaluation += target.Value * 0.1f;
        if (target.GetComponent<HeroScript>() != null)
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
    float ChancesToHit(UnitScript currentUnit,UnitScript target)
    {
        float EvaluationIncrease = 0f;
        HitChancer hc = new HitChancer(currentUnit, target, 100);  
         float damageChance = 100 - hc.MissChance(false);
        // im NOT sure if line above is correct, i had to change it after changing the combat system, it might be crap.
        float temp = damageChance * 0.01f;
        EvaluationIncrease += temp;
        return EvaluationIncrease;
    }
}
