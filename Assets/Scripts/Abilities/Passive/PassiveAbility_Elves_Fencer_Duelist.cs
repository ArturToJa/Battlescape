using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveAbility_Elves_Fencer_Duelist : PassiveAbility
{
    public override int GetAttack(UnitScript other)
    {
        if (myUnit.EnemyList.Count == 1)
        {
            return AttackModifierVersusUnitType;
        }
        else
        {
            return 0;
        }
    }

    public override int GetDefence(UnitScript other)
    {
        if (myUnit.EnemyList.Count == 1)
        {
            return DefenceModifierVersusUnitType;
        }
        else
        {
            return 0;
        }
    }

    protected override void ChangableStart()
    {
        return;
    }

    protected override void ChangableUpdate()
    {
        if (myUnit.EnemyList.Count == 1)
        {
            myUnit.isStoppingRetaliation = true;
        }
        else
        {
            myUnit.isStoppingRetaliation = false;
        }
    }
    
}
