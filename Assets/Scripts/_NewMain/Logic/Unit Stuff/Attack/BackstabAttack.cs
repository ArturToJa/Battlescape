using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattlescapeLogic
{
    public class BackstabAttack : AbstractAttack
    {
        AbstractAttack normalAttackType;
        Damage damage;

        public BackstabAttack(AbstractAttack _normalAttackType, Damage _damage, Unit _myUnit) : base(_myUnit)
        {
            sourceUnit = _myUnit;
            damage = _damage;
            normalAttackType = _normalAttackType;

            _myUnit.equipment.EquipMainMeleeWeapon();           
        }

        public override void BasicAttack(IDamageable target)
        {
            base.BasicAttack(target);
            TurnTowardsTarget();
            PlayAttackAnimation();
        }

        public override void OnAttackAnimation()
        {
            if (sourceUnit.GetMyOwner().type != PlayerType.Network)
            {
                Networking.instance.SendCommandToHit(targetObject, damage);
                sourceUnit.attack = normalAttackType;                
            }
        }      

        protected override void PlayAttackAnimation()
        {
            sourceUnit.animator.SetTrigger("Attack");
        }       
    }
}