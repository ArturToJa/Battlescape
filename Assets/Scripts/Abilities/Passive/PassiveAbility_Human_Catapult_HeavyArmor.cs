using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveAbility_Human_Catapult_HeavyArmor : PassiveAbility
{
    public override int GetAttack(UnitScript other)
    {
        if (AttackModifierVersusUnitType != 0)
        {
            Debug.LogError("why?");
        }
        return AttackModifierVersusUnitType;
    }

    public override int GetDefence(UnitScript other)
    {
        if (other.isRanged && TurnManager.Instance.CurrentPhase == TurnPhases.Shooting)
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
