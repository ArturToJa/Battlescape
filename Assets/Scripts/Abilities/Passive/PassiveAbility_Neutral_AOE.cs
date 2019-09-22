using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveAbility_Neutral_AOE : PassiveAbility
{
    public override int GetAttack(UnitScript other)
    {
        return 0;
    }

    public override int GetDefence(UnitScript other)
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
