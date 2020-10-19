using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BattlescapeLogic
{
    public class DamageCalculator
    {
        public static readonly float sigmoidGrowthRate = 0.05f;
        public static readonly int minimalDamageInGame = 1;



        public static Damage CalculateDamage(Unit source, IDamageable target, float multiplier = 1)
        {

            int averageDamage = GetAvarageDamage(source,target,multiplier);
            int damageRange = averageDamage / 5;
            int finalDamage = UnityEngine.Random.Range(averageDamage - damageRange, averageDamage + damageRange + 1);
            if (finalDamage < minimalDamageInGame)
            {
                finalDamage = minimalDamageInGame;
            }
            return new Damage(finalDamage, IsMiss(source, target));
        }

        public static bool IsMiss(Unit source, IDamageable target)
        {            
            return UnityEngine.Random.value > target.ChanceOfBeingHitBy(source);
        }                

        public static int GetStatisticsDifference(Unit source, IDamageable target)
        {            
            return (source.statistics.GetCurrentAttack() - target.GetCurrentDefence());
        }

        public static int GetAvarageDamage(Unit source, IDamageable target, float multiplier = 1)
        {
            return Mathf.RoundToInt((Statistics.baseDamage + ((float)GetStatisticsDifference(source, target)/10)) * multiplier);
        }
    }
}
