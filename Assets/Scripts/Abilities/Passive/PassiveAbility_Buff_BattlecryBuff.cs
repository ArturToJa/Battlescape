using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveAbility_Buff_BattlecryBuff : PassiveAbility_Buff
{
    public static IEnumerator AddMyBuff(BattlescapeLogic.Unit me, BattlescapeLogic.Unit ally, GameObject vfxTemporary, GameObject vfxPermanent, float timeDelay)
    {
        PassiveAbility_Buff_BattlecryBuff theBuff = ally.gameObject.AddComponent<PassiveAbility_Buff_BattlecryBuff>();

        theBuff.HasIcon = true;
        theBuff.AbilityIconName = "BattlecryBuff";
        theBuff.AttackBuffValue = 1;
        theBuff.DefenceBuffValue = 0;
        theBuff.MovementBuffValue = 0;
        theBuff.BuffDuration = 2;
        theBuff.IsFrozen = false;
        theBuff.isNegative = false;
        theBuff.watchForAttacksBy = me;
        theBuff.DoBuff();

        foreach (PassiveAbility_Buff buff in ally.gameObject.GetComponents<PassiveAbility_Buff>())
        {
            if (buff.isNegative)
            {
                buff.UndoBuff();
            }
        }

        if (vfxTemporary != null)
        {
            theBuff.vfx = Instantiate(vfxTemporary, ally.gameObject.transform.position, vfxTemporary.transform.rotation, ally.gameObject.transform);
        }

        yield return new WaitForSeconds(timeDelay);

        if (vfxPermanent != null)
        {
            theBuff.vfx = Instantiate(vfxPermanent, ally.gameObject.transform.position, vfxPermanent.transform.rotation, ally.gameObject.transform);
        }        
    }

    protected override void OnMasterAttacked()
    {
        //if (damage > 0)
        //{
        //    Log.SpawnLog(myUnit.name + " got inspired by his Knight's spectaculat attack on an enemy, gaining additional +1 Attack until his next turn!");
        //    TemporarilyUndooBuff();
        //    AttackBuffValue = 2;
        //    DoBuff();
        //}
    }    
}
