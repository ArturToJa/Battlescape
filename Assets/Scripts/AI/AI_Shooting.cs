using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BattlescapeLogic;

public class AI_Shooting : AI_Base_Attack
{
    public AI_Shooting(int ID) : base(ID)
    {
        base.CallTheConstructor(ID);
    }

    public override float EvaluateAsATarget(BattlescapeLogic.Unit currentUnit, Tile startingTile, Tile targetTile)
    {
        // here our shooter makes a decision of where to shoot at. He will:
        // 1. Want to hit guys with high "danger" to our units.
        // 2. Want to hit guys, who will give him more points
        // 3. Want to hit guys, who he has bigger chance to actually damage.

        BattlescapeLogic.Unit target = targetTile.myUnit;
        float Evaluation = 0f;
//        Debug.Log("Evaluating a shot by: " + currentUnit + " at: " + target);
        // 1.
        Evaluation += HowDangerousThisEnemyIs(target);

        // 2.
        Evaluation += target.statistics.cost* 0.1f;
        if (target is Hero)
        {
            Evaluation += 0.5f;
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

    float ChancesToHit(BattlescapeLogic.Unit currentUnit, BattlescapeLogic.Unit target)
    {
        float EvaluationIncrease = 0f;
        // above the Value not the Key of possible to shoot at is important. It is missleading yet correct.        
        float damageChance = 100; //- DamageCalculator.MissChance(currentUnit, target);
        // im NOT sure if line above is correct, i had to change it after changing the combat system, it might be crap.
        float temp = damageChance * 0.015f;
        EvaluationIncrease += temp;
        return EvaluationIncrease;
    }

    float HowDangerousThisEnemyIs(BattlescapeLogic.Unit enemy)
    {
        float EvaluationIncrease = 0f;
        EvaluationIncrease += enemy.statistics.GetCurrentAttack() * 0.1f * (enemy.statistics.healthPoints / enemy.statistics.maxHealthPoints);
//        Debug.Log("Evaluation increase against: " + enemy + " for his dangerousness is: " + EvaluationIncrease);
        return EvaluationIncrease;
    }

    protected override Queue<BattlescapeLogic.Unit> GetPossibleUnits()
    {        
        Queue<BattlescapeLogic.Unit> myUnitsToMove = new Queue<BattlescapeLogic.Unit>(allyList);
        BattlescapeLogic.Unit shooter = myUnitsToMove.Peek();        
        while (shooter == null || shooter.CanStillAttack() == false || shooter.IsInCombat())
        {
            myUnitsToMove.Dequeue();
           
            if (myUnitsToMove.Count == 0)
            {
                //GameStateManager.NextPhase();
                return null;
            }
            else
            {
                shooter = myUnitsToMove.Peek();
            }

        }
        return myUnitsToMove;
    }

    protected override void PerformTheAction(BattlescapeLogic.Unit currentUnit, KeyValuePair<Tile, float> target)
    {
        AI_Controller.tilesAreEvaluated = false;
        if (target.Key != null)
        {
            Debug.Log("Chosen tile is: " + target.Key);
            Unit attackTarget = target.Key.myUnit;
            currentUnit.statistics.numberOfAttacks = 0;
            Networking.instance.SendCommandToStartAttack(currentUnit, attackTarget);
        }
        else
        {
            currentUnit.statistics.numberOfAttacks = 0;
        }
    }

    public override List<Tile> GetPossibleMoves(BattlescapeLogic.Unit currentUnit, bool isAlly)
    {
        List<Tile> enemiesInRange = new List<Tile>();
        foreach (BattlescapeLogic.Unit enemy in enemyList)
        {
            bool isInRange = false; // CombatController.Instance.WouldItBePossibleToShoot(currentUnit, currentUnit.transform.position, enemy.transform.position);
            if (enemy.GetMyOwner().index != this.ID && isInRange && enemy.IsAlive())
            {
                enemiesInRange.Add(enemy.currentPosition);
            }
        }
        return enemiesInRange;
    }

    public  BattlescapeLogic.Unit FindBestTarget(BattlescapeLogic.Unit shooter, Vector3 startingPosition, int range)
    {
        // we want to get THE BEST target XD. Which for now means:
        // 1. Get all enemies we, from the tile, could shoot at as a Dictionary of units and floats.
        // 2. For each of them calculate their value (we will use it once again as shooting evaluation)
        // 3. Return the best one xD.

        Dictionary<BattlescapeLogic.Unit, float> TargetsEvaluated = new Dictionary<BattlescapeLogic.Unit, float>();
        List<BattlescapeLogic.Unit> AllEnemies = VictoryLossChecker.GetEnemyUnitList();
        Bounds Range = new Bounds(startingPosition, new Vector3(2 * range + 0.25f, 5, 2 * range + 0.25f));
        foreach (var enemy in AllEnemies)
        {            
            if (Range.Contains(enemy.transform.position))
            {
                TargetsEvaluated.Add(enemy, EvaluateAsATarget(shooter, enemy.currentPosition));
            }
        }

        if (TargetsEvaluated.Keys.Count == 0)
        {
            return null;
        }
        else
        {
            float highestValue = -Mathf.Infinity;
            BattlescapeLogic.Unit choice = null;
            foreach (var item in TargetsEvaluated)
            {
                if (item.Value > highestValue)
                {
                    highestValue = item.Value;
                    choice = item.Key;
                }
            }
           // Debug.Log("Shooting target for: " + shooter + " has been found!" + " Target is: " + choice + " Evaluated as: " + TargetsEvaluated[choice]);
            return choice;
        }
    }
}
