using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BattlescapeLogic;

public class AssassinStatModifier : PassiveAbility
{
    public override int GetAttack(BattlescapeLogic.Unit other)
    {
        if (IsActive(other))
        {
            return AttackModifierVersusUnitType;
        }

        else
        {
            return 0;
        }

    }

    public override int GetDefence(BattlescapeLogic.Unit other)
    {
        if (DefenceModifierVersusUnitType!= 0)
        {
            Debug.LogError("why?");
        }
        return DefenceModifierVersusUnitType;
    }

    protected override void ChangableStart()
    {
        return;
    }

    protected override void ChangableUpdate()
    {
        return;
    }

    bool IsActive(Unit other)
    {
        if (other is Hero)
        {
            return true;
        }
        //if (other.isColossal)
        //{
        //    return true;
        //}
        if (QCManager.Instance.IsTimeForBackstabs)
        {
            return true;
        }
        // TODO: if (IsLeavingDisguise) also return true in the future!
        else
            return false;
    }


}
