using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattlescapeLogic
{
    public abstract class AbstractAttackModifier : AbstractAbility
    {
        public Expirable expirable { get; private set; }

        public abstract void ModifyDamage(Damage damage);

        public abstract void ModifyAttack(IDamageable target, Damage damage);

        public virtual void ApplyToUnit(Unit unit)
        {
            owner = unit;
            owner.modifiers.Add(this);
            expirable = new Expirable(owner.GetMyOwner(), OnExpire);
        }

        protected void OnExpire()
        {
            owner.modifiers.Remove(this);
        }
    }
}