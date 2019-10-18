using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattlescapeLogic
{
    public abstract class AbstractPassiveAbility : AbstractAbility
    {
        public List<Buff> placeableBuffs { get; protected set; }

        protected void ApplyBuffsToUnit(Unit unit)
        {
            foreach (Buff buff in placeableBuffs)
            {
                //unit.applyBuff(buff);
            }
        }
    }
}