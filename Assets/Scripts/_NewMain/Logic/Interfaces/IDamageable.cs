﻿using UnityEngine;

namespace BattlescapeLogic
{
    public interface IDamageable: IMouseTargetable, IOnTilePlaceable
    {

        BuffGroup buffs { get;}

        string GetMyName();

        Player GetMyOwner();

        Vector3 GetMyPosition();

        void TakeDamage(Unit source, int dmg);

        int GetCurrentDefence();
        float ChanceOfBeingHitBy(Unit source);
    }
}
