using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattlescapeLogic
{
    public class ShieldBash : AbstractActiveUnitTargetAbility
    {
        [SerializeField] GameObject buffPrefab;
        [SerializeField] int baseDamage;
         
        public override void DoAbility()
        {
            ApplyBuffToUnit(buffPrefab, target);

            //this possibly should be in DamageCalculator;
            int damage = baseDamage + DamageCalculator.GetStatisticsDifference(owner, target);
            if (damage < DamageCalculator.minimalDamageInGame)
            {
                damage = DamageCalculator.minimalDamageInGame;
            }
            //up to here

            Networking.instance.SendCommandToHit(owner, target, damage);
        }
    }
}

