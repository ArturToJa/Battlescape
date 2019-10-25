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

    public override float EvaluateAsATarget(UnitScript currentUnit, Tile startingTile, Tile targetTile)
    {
        // here our shooter makes a decision of where to shoot at. He will:
        // 1. Want to hit guys with high "danger" to our units.
        // 2. Want to hit guys, who will give him more points
        // 3. Want to hit guys, who he has bigger chance to actually damage.

        ShootingScript shooter = currentUnit.GetComponent<ShootingScript>();
        UnitScript target = targetTile.myUnit;
        float Evaluation = 0f;
//        Debug.Log("Evaluating a shot by: " + currentUnit + " at: " + target);
        // 1.
        Evaluation += HowDangerousThisEnemyIs(target);

        // 2.
        Evaluation += target.Value* 0.1f;
        if (target.GetComponent<HeroScript>() != null)
        {
            Evaluation += 0.5f;
            // Thats just my ruff estimate of "value" of a hero here, lol.
        }
       // Debug.Log("Evaluation increased by: " + target.Value * 0.1f + " because of arget's value");
        // NOTE - this might want to get changed to "AI Value" when we add AIData scripts for units.

        // 3.
        float temp = ChancesToHit(currentUnit, shooter, target);
        Evaluation += temp;
       // Debug.Log("Evaluation increased by: " + temp + " for chances to hit");
        return Evaluation;
    }

    float ChancesToHit(UnitScript currentUnit, ShootingScript shooter, UnitScript target)
    {
        float EvaluationIncrease = 0f;
        HitChancer hc = new HitChancer(currentUnit, target, 100);
        bool badRange = ShootingScript.WouldItBePossibleToShoot(shooter, shooter.transform.position, target.transform.position).Value;
        // above the Value not the Key of possible to shoot at is important. It is missleading yet correct.        
        float damageChance = 100 - hc.MissChance(badRange);
        // im NOT sure if line above is correct, i had to change it after changing the combat system, it might be crap.
        float temp = damageChance * 0.015f;
        EvaluationIncrease += temp;
        return EvaluationIncrease;
    }

    float HowDangerousThisEnemyIs(UnitScript enemy)
    {
        float EvaluationIncrease = 0f;
        EvaluationIncrease += enemy.CurrentAttack * 0.1f * (enemy.CurrentHP / enemy.MaxHP);
//        Debug.Log("Evaluation increase against: " + enemy + " for his dangerousness is: " + EvaluationIncrease);
        return EvaluationIncrease;
    }

    protected override Queue<UnitScript> GetPossibleUnits()
    {        
        Queue<UnitScript> myUnitsToMove = new Queue<UnitScript>(allyList);
        UnitScript firstUnit = myUnitsToMove.Peek();
        ShootingScript shooter = firstUnit.GetComponent<ShootingScript>();
        while (shooter == null || shooter.CanShoot == false || firstUnit.CheckIfIsInCombat() == true)
        {
            myUnitsToMove.Dequeue();
           
            if (myUnitsToMove.Count == 0)
            {
                GameStateManager.NextPhase();
                return null;
            }
            else
            {
                firstUnit = myUnitsToMove.Peek();
                shooter = firstUnit.GetComponent<ShootingScript>();
            }

        }
        return myUnitsToMove;
    }

    protected override void PerformTheAction(UnitScript currentUnit, KeyValuePair<Tile, float> target)
    {
        AI_Controller.tilesAreEvaluated = false;
        if (target.Key != null)
        {
            Debug.Log("Chosen tile is: " + target.Key);
            CombatController.Instance.AttackTarget = target.Key.myUnit.GetComponent<UnitScript>();
            currentUnit.GetComponent<ShootingScript>().hasAlreadyShot = true;
            CombatController.Instance.Shoot(currentUnit, CombatController.Instance.AttackTarget, currentUnit.GetComponent<ShootingScript>().CheckBadRange(target.Key.gameObject),currentUnit.GetComponent<ShootingScript>().AOEAttack);
        }
        else
        {
            currentUnit.GetComponent<ShootingScript>().hasAlreadyShot = true;
        }
    }

    public override List<Tile> GetPossibleMoves(UnitScript currentUnit, bool isAlly)
    {
        List<Tile> enemiesInRange = new List<Tile>();
        foreach (UnitScript enemy in enemyList)
        {
            bool isInRange = ShootingScript.WouldItBePossibleToShoot(currentUnit.GetComponent<ShootingScript>(), currentUnit.transform.position, enemy.transform.position).Key;
            if (enemy.PlayerID != this.ID && isInRange && enemy.CurrentHP > 0)
            {
                enemiesInRange.Add(enemy.myTile);
            }
        }
        return enemiesInRange;
    }

    public  UnitScript FindBestTarget(ShootingScript shooter, Vector3 startingPosition, int range)
    {
        // we want to get THE BEST target XD. Which for now means:
        // 1. Get all enemies we, from the tile, could shoot at as a Dictionary of units and floats.
        // 2. For each of them calculate their value (we will use it once again as shooting evaluation)
        // 3. Return the best one xD.

        Dictionary<UnitScript, float> TargetsEvaluated = new Dictionary<UnitScript, float>();
        List<UnitScript> AllEnemies = VictoryLossChecker.GetEnemyUnitList();
        Bounds Range = new Bounds(startingPosition, new Vector3(2 * range + 0.25f, 5, 2 * range + 0.25f));
        foreach (var enemy in AllEnemies)
        {            
            if (Range.Contains(enemy.transform.position))
            {
                TargetsEvaluated.Add(enemy, EvaluateAsATarget(shooter.GetComponent<UnitScript>(), enemy.myTile));
            }
        }

        if (TargetsEvaluated.Keys.Count == 0)
        {
            return null;
        }
        else
        {
            float highestValue = -Mathf.Infinity;
            UnitScript choice = null;
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
