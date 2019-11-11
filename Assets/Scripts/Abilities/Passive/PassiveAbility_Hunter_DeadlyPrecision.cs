using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveAbility_Hunter_DeadlyPrecision : PassiveAbility
{
    public override int GetAttack(BattlescapeLogic.Unit other)
    {
        return 0;
    }

    public override int GetDefence(BattlescapeLogic.Unit other)
    {
        return 0;
    }

    protected override void ChangableStart()
    {
        return;
    }

    protected override void ChangableUpdate()
    {
        return;
    }
}
