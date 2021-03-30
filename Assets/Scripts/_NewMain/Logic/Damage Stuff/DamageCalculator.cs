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
        public static readonly int damageDivisorForRange = 5;
        public static readonly float damageMultiplierForBackstabs = 1.5f;



        public static Damage CalculateBasicAttackDamage(IDamageSource source, IDamageable target, float multiplier = 1)
        {

            int averageDamage = GetAvarageDamage(Statistics.baseDamage, source, target, multiplier);
            int damageRange = averageDamage / damageDivisorForRange;
            int finalDamage = UnityEngine.Random.Range(averageDamage - damageRange, averageDamage + damageRange + 1);
            if (finalDamage < minimalDamageInGame)
            {
                finalDamage = minimalDamageInGame;
            }
            return new Damage(finalDamage, IsHit(source, target), source);
        }

        public static Damage CalculateAbilityDamage(int baseDamage, IDamageSource source, IDamageable target)
        {
            int damage = GetAvarageDamage(baseDamage, source, target);

            return new Damage(damage, true, source);
        }

        public static bool IsHit(IDamageSource source, IDamageable target)
        {
            return UnityEngine.Random.value <= target.ChanceOfBeingHitBy(source);
        }

        public static int GetStatisticsDifference(IDamageSource source, IDamageable target)
        {
            return (source.GetAttackValue() - target.GetCurrentDefence()) / 10;
        }

        public static int GetAvarageDamage(int baseDamage, IDamageSource source, IDamageable target, float multiplier = 1)
        {
            int damage = Mathf.RoundToInt((baseDamage + ((float)GetStatisticsDifference(source, target))) * multiplier);
            if (damage < minimalDamageInGame)
            {
                damage = minimalDamageInGame;
            }
            return damage;
        }

        public static PotentialDamage GetPotentialDamage(int baseDamage, IDamageSource source, IDamageable target)
        {
            int avarageDamage = GetAvarageDamage(baseDamage, source, target);
            int damageHalfRange = avarageDamage / damageDivisorForRange;
            float hitChance = target.ChanceOfBeingHitBy(source);
            return new PotentialDamage(avarageDamage, damageHalfRange, hitChance);
        }
    }
}
