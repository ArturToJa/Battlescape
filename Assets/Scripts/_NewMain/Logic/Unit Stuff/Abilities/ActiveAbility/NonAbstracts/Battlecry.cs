using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattlescapeLogic
{
    public class Battlecry : AbstractActiveNoTargetAbility
    {
        [SerializeField] BattlecryBuff buffPrefab;
        public override void DoAbility()
        {
            foreach (Unit ally in owner.GetMyOwner().playerUnits)
            {
                if (IsInRange(ally) && filter.FilterUnit(ally))
                {
                   var temp = ApplyBuffToUnit(buffPrefab, ally);
                    Damage.OnDamageDealt += (temp as BattlecryBuff).OnDamageDealt;
                }
            }
        }

        protected override string GetLogMessage()
        {
            return base.GetLogMessage() + " It increases Attack and Movement of allies in range for." + buffPrefab.GetDuration() + " rounds." + "\n" + "If " + owner.info.unitName + "deals damage, increase the attack of those allies by 20%";
        }
        
    }
}