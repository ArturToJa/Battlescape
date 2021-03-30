using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattlescapeLogic
{
    //THIS is currently either an AbstractActiveAbility or an AbstractAttack. NOT a unit. If something is that - it WILL trigger HitChance tooltip;
    public interface IDamageSource
    {
        PotentialDamage GetPotentialDamageAgainst(IDamageable target);

        bool CanPotentiallyDamage(IDamageable target);

        int GetAttackValue();

        string GetOwnerName();

        ModifierGroup GetMyDamageModifiers();

        void OnKillUnit(Unit unit);
    }
}