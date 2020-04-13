using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BattlescapeLogic;

public class PassiveAbility_Elves_Fencer_Duelist : PassiveAbility
{
    public override int GetAttack(BattlescapeLogic.Unit other)
    {
        if (HasOneEnemy())
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
        if (HasOneEnemy())
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
        if (HasOneEnemy())
        {
            //myUnit.isStoppingRetaliation = true;
        }
        else
        {
            //myUnit.isStoppingRetaliation = false;
        }
    }

    bool HasOneEnemy()
    {
        int count = 0;
        //foreach (Tile tile in myUnit.currentPosition.neighbours)
        //{
        //    if (tile.GetMyObject<Unit>() != null && tile.GetMyObject<Unit>().GetMyOwner() != myUnit.GetMyOwner())
        //    {
        //        count++;
        //    }
        //}
        return count == 1;
    }
    
}
