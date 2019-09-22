using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssassinStatModifier : PassiveAbility
{
    public override int GetAttack(UnitScript other)
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

    public override int GetDefence(UnitScript other)
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

    bool IsActive(UnitScript other)
    {
        if (other.GetComponent<HeroScript>() != null)
        {
            return true;
        }
        if (other.isColossal)
        {
            return true;
        }
        if (QCManager.Instance.IsTimeForBackstabs)
        {
            return true;
        }
        // TODO: if (IsLeavingDisguise) also return true in the future!
        else
            return false;
    }


}
