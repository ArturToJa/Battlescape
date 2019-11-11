using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattlescapeLogic
{
    public abstract class AbstractPassiveAbility : AbstractAbility
    {
        public List<AbstractBuff> placeableBuffs { get; protected set; }

        protected void ApplyBuffsToUnit(Unit unit)
        {
            foreach (AbstractBuff buff in placeableBuffs)
            {
                //unit.applyBuff(buff);
            }
        }
    }
}