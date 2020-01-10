using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BattlescapeLogic
{
    public class DamageCalculator
    {
        public static readonly float sigmoidGrowthRate = 0.5f;

        public static int CalculateBasicDamage(Unit source, Unit target)
        {
            if (IsMiss(source,target))
            {
                return 0;
            }
            int averageDamage = Statistics.baseDamage + GetStatisticsDifference(source, target);
            int damageRange = averageDamage / 5;
            return UnityEngine.Random.Range(averageDamage - damageRange, averageDamage + damageRange + 1);
        }

        public static bool IsMiss(Unit source, Unit target)
        {
            return UnityEngine.Random.Range(0.0f, 1.0f) > HitChance(source, target);
        }

        public static float HitChance(Unit source, Unit target)
        {
            return Maths.Sigmoid(GetStatisticsDifference(source, target), sigmoidGrowthRate);
        }

        public static int GetStatisticsDifference(Unit source, Unit target)
        {
            int totalAttack = source.statistics.baseAttack + source.statistics.bonusAttack;
            int totalDefence = target.statistics.baseDefence + target.statistics.bonusDefence;
            return totalAttack - totalDefence;
        }
    }
}