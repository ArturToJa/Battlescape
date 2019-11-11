using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveAbility_Human_Catapult_HeavyArmor : PassiveAbility
{
    public override int GetAttack(BattlescapeLogic.Unit other)
    {
        if (AttackModifierVersusUnitType != 0)
        {
            Debug.LogError("why?");
        }
        return AttackModifierVersusUnitType;
    }

    public override int GetDefence(BattlescapeLogic.Unit other)
    {
        if (other.IsRanged() && other.currentPosition.neighbours.Contains(myUnit.currentPosition) == false)
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
        return;
    }




}
