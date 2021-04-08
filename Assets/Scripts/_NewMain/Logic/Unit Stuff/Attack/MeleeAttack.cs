using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattlescapeLogic
{

    public class MeleeAttack : AbstractAttack
    {

        public MeleeAttack(Unit _myUnit) : base(_myUnit)
        {
            sourceUnit = _myUnit;
            _myUnit.equipment.EquipMainMeleeWeapon();
        }

        public override void BasicAttack(IDamageable target)
        {
            base.BasicAttack(target);
            TurnTowardsTarget();
            PlayAttackAnimation();
        }
        protected override void PlayAttackAnimation()
        {
            sourceUnit.animator.SetTrigger("Attack");
        }

        public override void OnAttackAnimation()
        {
            //IDK if this needs to even be virtual because im not thinking anymore as it is very late at night when i'm coiding it.
            //Here we would calculate the damage.
            //IDK how much should be done here, and how much should be done on the unit's side (deal dmg vs get dmg)
            if (sourceUnit.GetMyOwner().type != PlayerType.Network)
            {
                Networking.instance.SendCommandToHit(targetObject, DamageCalculator.CalculateBasicAttackDamage(this, targetObject));
            }
        }

        public override void Backstab(IDamageable target)
        {
            sourceUnit.attack = new BackstabAttack(this, sourceUnit);
            sourceUnit.attack.Backstab(target);
        }
    }
}
