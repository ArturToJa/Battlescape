using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattlescapeLogic
{
    public class ShieldBash : AbstractActiveUnitTargetAbility, IDamageSource
    {
        [SerializeField] StateBuff buffPrefab;
        [SerializeField] int baseDamage;

        public Unit GetMyOwner()
        {
            return owner;
        }

        public override void DoAbility()
        {
            ApplyBuffToUnit(buffPrefab, target);
            owner.statistics.numberOfAttacks--;
            Damage damage = DamageCalculator.CalculateAbilityDamage(baseDamage, this, target);

            Networking.instance.SendCommandToHit(target, damage);
        }

        public PotentialDamage GetPotentialDamageAgainst(IDamageable target)
        {
            if ((target is Unit) == false)
            {
                Debug.LogError("Wrong target type!");
                return new PotentialDamage(0, 0, 0);
            }
            else
            {
                int damageValue = DamageCalculator.CalculateAbilityDamage(baseDamage, this, target);
                int damageHalfRange = 0;
                float hitChance = 1;
                return new PotentialDamage(damageValue, damageHalfRange, hitChance);
            }
        }

        public bool CanPotentiallyDamage(IDamageable target)
        {
            //perhaps could be done better but hey
            //perhaps the 'acts on units' will be a part of filter in the future? idk.
            return filter.FilterTarget(target) && target is Unit && target.IsAlive();
        }

        public void OnKillUnit(Unit unit)
        {
            if (owner.IsEnemyOf(unit))
            {
                owner.GetMyOwner().AddPoints(unit.statistics.cost);
            }
        }

        public override Color GetColourForTargets()
        {
            return Global.instance.colours.red;
        }

        protected override string GetLogMessage()
        {
            return base.GetLogMessage() + "\n" + "Target is stunned for " + buffPrefab.GetDuration() + " rounds and receives damage!";
        }
    }
}

