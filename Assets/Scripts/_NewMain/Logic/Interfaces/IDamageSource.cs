﻿using System;

namespace BattlescapeLogic
{
    //THIS is currently either an AbstractActiveAbility or an AbstractAttack. NOT a unit. If something is that - it WILL trigger HitChance tooltip;
    public interface IDamageSource
    {
        Unit GetMyOwner();

        PotentialDamage GetPotentialDamageAgainst(IDamageable target);

        bool CanPotentiallyDamage(IDamageable target);               

        void OnKillUnit(Unit unit);

    }
}