using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BasicStatModifier : PassiveAbility
{
    public UnitType favouriteEnemy = UnitType.Null;

    public override int GetAttack(UnitScript target)
    {
        if (favouriteEnemy == UnitType.Null)
        {
            return 0;
        }

        if (target.unitType == favouriteEnemy)
        {
            return AttackModifierVersusUnitType;
        }
        else
        {
            return 0;
        }
    }

    public override int GetDefence(UnitScript enemy)
    {
        int actualDefence = 0;
        if (TurnManager.Instance.CurrentPhase == TurnPhases.Shooting)
        {
            actualDefence += myUnit.HowMuchShelteredFrom(enemy.transform.position);
        }
        if (favouriteEnemy == UnitType.Null)
        {
            return actualDefence;
        }
        if (enemy.unitType == favouriteEnemy)
        {
            return actualDefence + DefenceModifierVersusUnitType;
        }
        else
        {
            return actualDefence;
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
