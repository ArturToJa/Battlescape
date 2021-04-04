using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattlescapeLogic
{
    public class Test
    {
        

        public void TestHitChancesForDifferentStatValues()
        {
            Debug.Log("Testing hit chances for attack and defence values.");
            TestHitChanceForStatValues(0, 0);
            TestHitChanceForStatValues(1, 0);
            TestHitChanceForStatValues(10, 0);
            TestHitChanceForStatValues(20, 0);
            TestHitChanceForStatValues(30, 0);
            TestHitChanceForStatValues(40, 0);
            TestHitChanceForStatValues(50, 0);
            TestHitChanceForStatValues(60, 0);
            TestHitChanceForStatValues(100, 0);
        }
        void TestHitChanceForStatValues(int attack, int defence)
        {
            Debug.Log("Attack: " + attack + ", Defence: " + defence + ". Result: " + Maths.Sigmoid(attack - defence, DamageCalculator.sigmoidGrowthRate));
        }

        public void TestDamageForDifferentStatValues()
        {
            Debug.Log("Testing damage values for attack and defence values.");
            TestDamageForStatValues(0, 0);
            TestDamageForStatValues(1, 0);
            TestDamageForStatValues(10, 0);
            TestDamageForStatValues(20, 0);
            TestDamageForStatValues(30, 0);
            TestDamageForStatValues(40, 0);
            TestDamageForStatValues(50, 0);
            TestDamageForStatValues(60, 0);
            TestDamageForStatValues(100, 0);
        }

        void TestDamageForStatValues(int attack, int defence)
        {
            IDamageSource mockAttacker = new FakeDamageSource(attack);
            IDamageable mockDefender = new FakeDamageTarget(defence);
            PotentialDamage damage = DamageCalculator.GetPotentialDamage(Statistics.baseDamage, mockAttacker, mockDefender);
            Debug.Log("Attack: " + attack + ", Defence: " + defence + ". Result: " + (damage.avarageDamage-damage.damageHalfRange).ToString() + " - " + (damage.avarageDamage + damage.damageHalfRange).ToString());
        }

        public void RunFullTests()
        {
            TestHitChancesForDifferentStatValues();
            TestDamageForDifferentStatValues();
        }
    }
}