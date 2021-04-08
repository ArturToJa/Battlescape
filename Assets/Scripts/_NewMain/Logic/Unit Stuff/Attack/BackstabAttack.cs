using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattlescapeLogic
{
    public class BackstabAttack : AbstractAttack
    {
        AbstractAttack normalAttackType;

        public BackstabAttack(AbstractAttack _normalAttackType, Unit _myUnit) : base(_myUnit)
        {
            sourceUnit = _myUnit;
            normalAttackType = _normalAttackType;

            _myUnit.equipment.EquipMainMeleeWeapon();           
        }

        public override void BasicAttack(IDamageable target)
        {
            Debug.LogWarning("Backstab attack is not supposed to exist and normally attack. It should only backstab and disappear immidiately!");
        }

        public override void OnAttackAnimation()
        {
            if (sourceUnit.GetMyOwner().type != PlayerType.Network)
            {
                Damage damage = DamageCalculator.CalculateBasicAttackDamage(sourceUnit.attack, targetObject, DamageCalculator.damageMultiplierForBackstabs);
                Networking.instance.SendCommandToHit(targetObject, damage);
                sourceUnit.attack = normalAttackType;                
            }
        }      

        protected override void PlayAttackAnimation()
        {
            sourceUnit.animator.SetTrigger("Attack");
        }
        public override void Backstab(IDamageable target)
        {
            base.BasicAttack(target);
            TurnTowardsTarget();
            PlayAttackAnimation();
        }
    }
}