using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BattlescapeLogic
{
    public class DamageCalculator
    {
        public static readonly float sigmoidGrowthRate = 0.5f;



        public static int CalculateDamage(Unit source, IDamageable target, float multiplier = 1)
        {
            if (IsMiss(source, target))
            {
                return 0;
            }
            int averageDamage = Mathf.RoundToInt((Statistics.baseDamage + GetStatisticsDifference(source, target)) * multiplier);
            int damageRange = averageDamage / 5;
            return UnityEngine.Random.Range(averageDamage - damageRange, averageDamage + damageRange + 1);
        }

        public static bool IsMiss(Unit source, IDamageable target)
        {
            if (target is DestructibleObstacle)
            {
                return false;
            }
            return UnityEngine.Random.value > HitChance(source, target);
        }        

        public static float HitChance(Unit source, IDamageable target)
        {
            return Maths.Sigmoid(GetStatisticsDifference(source, target), sigmoidGrowthRate);
        }

        public static int GetStatisticsDifference(Unit source, IDamageable target)
        {            
            return source.statistics.GetCurrentAttack() - target.GetCurrentDefence();
        }
    }
}