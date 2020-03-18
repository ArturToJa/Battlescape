using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattlescapeLogic
{
    public class ActiveAttackAbility : AbstractActiveUnitTargetAbility
    {
        [SerializeField] int _damage;
        public int damage
        {
            get
            {
                return _damage;            
            }
            protected set
            {
                _damage = value;
            }
        }

        public override bool IsUsableNow()
        {
            return base.IsUsableNow() && owner.CanStillAttack();

        }

        protected override void Activate()
        {
            base.Activate();
            ApplyBuffsToUnit(selfBuffs, owner);
            ApplyBuffsToUnit(targetBuffs, target as Unit);
            owner.attack = new AbilityAttack(this);
            owner.attack.Attack(target as Unit);
        }
                

        bool IsReady()
        {
            return PlayerInput.instance.isInputBlocked = false;
        }
    }
}