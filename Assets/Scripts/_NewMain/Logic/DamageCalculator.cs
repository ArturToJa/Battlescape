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

        public static int CalculateBackstabDamage(Unit source, Unit target)
        {
            float percentageChance = 0.2f * (source.CompareUnitClass(target) + 3);
            //Classes compared give number from +2 (hero/special unit attacks cannonmeat) to -2 (opposite attack), so 5 different options including 0.
            // If we add +3 to class, we have a number from 1 to 5. We multiply 20% times that number and we get a percentage chance of success from 20% to a 100% ;)
            float random = UnityEngine.Random.Range(0, 1);
            if (random <= percentageChance)
            {
                int averageDamage = Statistics.baseDamage + GetStatisticsDifference(source, target);
                int damageRange = averageDamage / 5;
                return UnityEngine.Random.Range(averageDamage - damageRange, averageDamage + damageRange + 1);
            }
            else
            {
                return 0;
            }
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