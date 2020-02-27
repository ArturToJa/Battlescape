using System.Collections;
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

            if (_myUnit.meleeWeaponVisual != null)
            {
                _myUnit.meleeWeaponVisual.SetActive(true);
            }
        }

        public override void Attack(Unit target)
        {
            base.Attack(target);
            TurnTowardsTarget();
            PlayAttackAnimation();
        }

        public override void OnAttackAnimation()
        {
            if (sourceUnit.owner.type != PlayerType.Network)
            {
                Networking.instance.SendCommandToHit(sourceUnit, targetUnit, damage);
                sourceUnit.attack = normalAttackType;                
            }
        }

        public override void OnRangedAttackAnimation()
        {
            return;
        }

        protected override void PlayAttackAnimation()
        {
            sourceUnit.animator.SetTrigger("Attack");
        }

        protected override void TurnTowardsTarget()
        {
            sourceUnit.visuals.transform.LookAt(new Vector3(targetUnit.transform.position.x, sourceUnit.visuals.transform.position.y, targetUnit.transform.position.z));
        }
    }
}