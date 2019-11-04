using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BattlescapeLogic;

[System.Serializable]
public class DamageCalculator
{
    // int tresholdKicker;    
    int diceFaceNumber;
    int damageForRandomization;

    public DamageCalculator()
    {
        if (CombatController.Instance != null)
        {
            // tresholdKicker = CombatController.Instance.TresholdKicker;
            diceFaceNumber = CombatController.Instance.DiceFaceNumber;
            damageForRandomization = CombatController.Instance.DamageForRandomization;
        }
        else
        {
            // this shall not happen in real situation but it does happen all the time on compile thats hwy i wrote it cause it was making me pissed off. 
        }
    }

    public bool DealDamage(UnitScript attacker, UnitScript target, int damage, bool isPoisoned, bool Retaliatable, int hits)
    {

        if (CheckIfDamageBlocked(attacker, target))
        {
            PopupTextController.AddPopupText("Blocked!", PopupTypes.Damage);
            Log.SpawnLog(attacker.name + " attacks " + target.name + " but the attack gets blocked!");
        }

        if (hits >= 2)
        {

            Log.SpawnLog(attacker.name + " attacks " + target.name + "! " + damage + " point(s) of damage is dealt.");
            PopupTextController.AddParalelPopupText("-" + damage.ToString(), PopupTypes.Damage);
        }
        if (hits <= 1)
        {
            Log.SpawnLog(attacker.name + " attacks " + target.name + ", but only reduces their Defence by 1!");
           // PopupTextController.AddParalelPopupText("Hit!", PopupTypes.Info);
            PopupTextController.AddParalelPopupText("-1 Defence", PopupTypes.Stats);
        }

        /*if (hits == 0)
        {
            Log.SpawnLog(attacker.name + " attacks " + target.name + ", but misses completely.");
            PopupTextController.AddPopupText("Miss!", PopupTypes.Info);
        }*/
        bool isShot = attacker.isRanged && (GameStateManager.Instance.GameState != GameStates.RetaliationState && GameStateManager.Instance.GameState != GameStates.AttackState) == false;

        bool didDie = target.DealDamage(damage, hits > 0, isPoisoned, isShot);
        if (target.CanCurrentlyRetaliate && target.IsAlive() && Retaliatable && Global.instance.playerTeams[target.PlayerID].players[0].type == PlayerType.Local)
        {
            GameStateManager.Instance.StartRetaliationChoice();
        }

        return didDie;
    }

    public int GetHits(UnitScript attacker, UnitScript defender, bool badRangeShooting)
    {
        if (CheckIfDamageBlocked(attacker, defender))
        {
            return 0;
        }
        else
        {
            return ThrowDice(attacker, defender, badRangeShooting);
        }

    }

    public int CalculateAmountOfDamage(UnitScript attacker, UnitScript defender, bool badRangeShooting, int hits)
    {
        /*    if (hits == 0)
            {
                return 0;
            }
          */
        if (hits <= 1)
        {
            CombatController.Instance.SendCommandToReduceDefence(defender, 1);
            return 0;
        }
        else
        {
            return GetDmgWithProbability(attacker, defender);
            //return GetDmgAtRandom(attacker, defender);
            //return attacker.CurrentDamage + attacker.statistics.currentAttack - defender.statistics.GetCurrentDefence();
        }
    }

    /*public int CalculateDamageForCalculations(UnitScript attacker, UnitScript defender, bool badRangeShooting, int treshold, int diceNumber)
    {
        if (CheckIfDamageBlocked(attacker, defender))
        {
            return 0;
        }
        else
        {
            int hits = ThrowDice(attacker, defender, badRangeShooting, treshold, diceNumber);
            if (hits >= 2)
            {
                return GetDmgAtRandom(attacker, defender);
               // return attacker.CurrentDamage + attacker.statistics.currentAttack - defender.statistics.GetCurrentDefence();
            }
        }
        return 0;
    }*/


    /* THIS IS OLD VERSION
    public int GetDmgWithProbability(UnitScript attacker, UnitScript defender)
    {
        int baseDmg = attacker.CurrentDamage + attacker.statistics.currentAttack - defender.statistics.GetCurrentDefence();
        int a = baseDmg / 5;
        int iterations = 0;
        int sum = (1 + a) / 2 * a;
        int fullSum = 2 * sum + 1 + a;
        int seed = Random.Range(1, fullSum + 1); //rand%fullSum + 1 whatever that means...
        int left = 1;
        int right = 1;
        for (int i = 0; i < a; i++)
        {
            if (seed >= left && seed <= right)
            {
  //              Debug.Log("returned: " + (baseDmg + iterations - a));
                return baseDmg + iterations - a;
            }
            left = right + 1;
            right = left + i + 1;
            iterations++;
        }
        for (int i = a-1; i >= -1; i--)
        {
            if (seed >= left && seed <= right)
            {
//                Debug.Log("returned: " + (baseDmg + iterations - a));
                return baseDmg + iterations - a;
            }
            left = right + 1;
            right = left + i + 1;
            iterations++;
        }
        Debug.LogError("WTF it didnt work!");
        return baseDmg;
    }*/

    // THIS is NEW VERSION

    public int GetDmgWithProbability(UnitScript attacker, UnitScript defender)
    {
        int baseDmg = Statistics.baseDamage + attacker.statistics.GetCurrentAttack() - defender.statistics.GetCurrentDefence();

        int minDmg = GetMiniDmg(attacker, defender);

        int maxDmg = GetMaxiDmg(attacker, defender);

        if (baseDmg % 2 == 1)
        {
            if (minDmg %2 == 1)
            {
                return RollDice(minDmg + 1, maxDmg + 1) - 1;
            }
            else
            {
                return RollDice(minDmg + 1, maxDmg + 1);
            }
        }
        else
        {
            return RollDice(minDmg, maxDmg);
        }
    }

    int RollDice(int min, int max)
    {
        return Random.Range(min/2, (max)/2 +1) + Random.Range(min/2, (max)/2 + 1);
    }


    // THIS is not used obviously currently, but might be in the future!
    int GetDmgAtRandom(UnitScript attacker, UnitScript defender)
    {
        int minDmg = GetMiniDmg(attacker, defender);
        int maxDmg = GetMaxiDmg(attacker, defender);
        return Random.Range(minDmg, maxDmg + 1);
    }

    public int GetMiniDmg(UnitScript attacker, UnitScript defender)
    {
        int baseDmg = GetBaseDmg(attacker, defender);
        int howManyTimesBaseValue = baseDmg / damageForRandomization;
        return baseDmg - howManyTimesBaseValue;
    }
    public int GetMaxiDmg(UnitScript attacker, UnitScript defender)
    {
        int baseDmg = GetBaseDmg(attacker, defender);
        int howManyTimesBaseValue = baseDmg / damageForRandomization;
        return baseDmg + howManyTimesBaseValue;
    }

    public int GetBaseDmg(UnitScript Attacker, UnitScript Defender)
    {
        int Attack = Attacker.statistics.GetCurrentAttack();
        foreach (PassiveAbility passive in Attacker.GetComponents<PassiveAbility>())
        {
            Attack += passive.GetAttack(Defender);
        }
        int Defence = Defender.statistics.GetCurrentDefence();
        foreach (PassiveAbility passive in Defender.GetComponents<PassiveAbility>())
        {
            Defence += passive.GetDefence(Attacker);
        }
        return Statistics.baseDamage + Attack - Defence;
    }

    bool CheckIfDamageBlocked(UnitScript attacker, UnitScript defender)
    {
        if (defender.BlocksHits)
        {
            return true;
        }

        return false;
    }

    public int CalculateTreshold(UnitScript attacker, UnitScript defender)
    {
        int Treshold = diceFaceNumber / 2 + 1;
        Treshold -= attacker.statistics.GetCurrentAttack();
        Treshold += defender.statistics.GetCurrentDefence();

        foreach (PassiveAbility passive in attacker.GetComponents<PassiveAbility>())
        {
            Treshold -= passive.GetAttack(defender);
        }
        foreach (PassiveAbility passive in defender.GetComponents<PassiveAbility>())
        {
            Treshold += passive.GetDefence(attacker);
        }
        return Treshold;
    }

    public int CalculateDiceNumber(UnitScript attacker, UnitScript defender, bool badRangeShooting)
    {
        int diceNumber = 3;
        /*if (attacker.statistics.healthPoints * 2 <= attacker.statistics.maxHealthPoints)
        {
            diceNumber--;
        }
        if (defender.statistics.healthPoints * 2 <= defender.statistics.maxHealthPoints)
        {
            diceNumber++;
        }*/


        if ((attacker.GetComponent<HeroScript>() != null || attacker.isSpecial) && defender.unitType == UnitType.Cannonmeat)
        {
            diceNumber++;
        }
        if ((defender.GetComponent<HeroScript>() != null || defender.isSpecial) && attacker.unitType == UnitType.Cannonmeat)
        {
            diceNumber--;
        }
        if (badRangeShooting)
        {
            diceNumber--;
        }
        if (diceNumber < 1)
        {
            diceNumber = 1;
        }
        return diceNumber;

    }

    public int ThrowDice(UnitScript attacker, UnitScript defender, bool badRangeShooting)
    {
        int dice = CalculateDiceNumber(attacker, defender, badRangeShooting);
        int hits = 0;

        //Debug.Log("Dice number: " + dice);
        int treshold = CalculateTreshold(attacker, defender);
        // Debug.Log("Treshold is: " + treshold);

        for (int i = 0; i < dice; i++)
        {
            int result = Random.Range(1, diceFaceNumber + 1);
            if ((result != 1 && result >= treshold || result == diceFaceNumber))
            {
                hits++;
            }
        }
        return hits;
    }

    public int ThrowDice(UnitScript attacker, UnitScript defender, bool badRangeShooting, int treshold, int diceNumber)
    {
        int hits = 0;
        for (int i = 0; i < diceNumber; i++)
        {
            int result = Random.Range(1, diceFaceNumber + 1);
            if ((result != 1 && result >= treshold || result == diceFaceNumber))
            {
                hits++;
            }
        }
        return hits;
    }

    /* public int GetSumOfDamage(int[] damage)
     {
         if (damage == null)
         {
             return 0;
         }
         int sum = 0;
         for (int i = 0; i < damage.Length; i++)
         {
             sum += damage[i];
         }
         return sum;
     }*/
}
