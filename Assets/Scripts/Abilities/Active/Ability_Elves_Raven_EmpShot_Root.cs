using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability_Elves_Raven_EmpShot_Root : Ability_Elves_Raven_EmpoweredShot
{
    [SerializeField] GameObject DebuffVfx;
    [SerializeField] int duration;
    public override void Activate()
    {
        base.Activate();
        StartCoroutine(Root());
    }

    IEnumerator Root()
    {
        Log.SpawnLog("Raven empowers his shot with 'Frost' spell, disabling all the " + Target.myUnit.name + "'s actions for next turn");
        yield return null;
        FinishUsing();
        PassiveAbility_Buff.AddBuff(Target.myUnit.gameObject, 2, 0, 0, 0, myUnit.statistics.currentMaxNumberOfRetaliations, "FrozenDebuff", DebuffVfx, 3f, true, true, false);
    }
}
