using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BattlescapeLogic
{
    public class DamageCalculator
    {
        public static readonly int diceSideNumber = 12;
        public static readonly int baseDamage = 10;
        public static int CalculateBasicDamage(Unit source, Unit target)
        {
            int statisticsModifier = (target.statistics.baseDefence + target.statistics.bonusDefence) - (source.statistics.baseAttack + source.statistics.bonusAttack);
            int averageDamage = baseDamage + statisticsModifier;
            int damageRange = averageDamage / 5;
            return UnityEngine.Random.Range(averageDamage - damageRange, averageDamage + damageRange + 1);
        }

        public static bool IsMiss(Unit source, Unit target)
        {
            int statisticsModifier = (target.statistics.baseDefence + target.statistics.bonusDefence) - (source.statistics.baseAttack + source.statistics.bonusAttack);
            int drawDiceOne = UnityEngine.Random.Range(1, diceSideNumber + 1);
            int drawDiceTwo = UnityEngine.Random.Range(1, diceSideNumber + 1);
            int drawDiceThree = UnityEngine.Random.Range(1, diceSideNumber + 1);

            bool diceOneHighHit = (drawDiceOne > (diceSideNumber / 2) + statisticsModifier);
            bool diceTwoHighHit = (drawDiceTwo > (diceSideNumber / 2) + statisticsModifier);
            bool diceThreeHighHit = (drawDiceThree > (diceSideNumber / 2) + statisticsModifier);

            return !((diceOneHighHit && diceTwoHighHit) || (diceTwoHighHit && diceThreeHighHit) || (diceOneHighHit && diceThreeHighHit));
    }
    }
}