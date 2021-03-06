﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattlescapeLogic
{
    public class BackstabAttack : AbstractAttack
    {
        AbstractAttack normalAttackType;
        int damage;

        public BackstabAttack(AbstractAttack _normalAttackType, int _damage, Unit _myUnit) : base(_myUnit)
        {
            sourceUnit = _myUnit;
            damage = _damage;
            normalAttackType = _normalAttackType;

            _myUnit.equipment.EquipMainMeleeWeapon();           
        }

        public override void Attack(IDamageable target)
        {
            base.Attack(target);
            TurnTowardsTarget();
            PlayAttackAnimation();
        }

        public override void OnAttackAnimation()
        {
            if (sourceUnit.GetMyOwner().type != PlayerType.Network)
            {
                Networking.instance.SendCommandToHit(sourceUnit, targetObject, damage);
                sourceUnit.attack = normalAttackType;                
            }
        }      

        protected override void PlayAttackAnimation()
        {
            sourceUnit.animator.SetTrigger("Attack");
        }       
    }
}