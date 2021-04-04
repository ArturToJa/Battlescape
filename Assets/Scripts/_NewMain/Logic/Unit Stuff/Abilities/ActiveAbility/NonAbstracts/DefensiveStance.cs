using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattlescapeLogic
{
    public class DefensiveStance : AbstractActiveNoTargetAbility
    {
        [SerializeField] AbstractBuff buffPrefab;
        public override void DoAbility()
        {
            ApplyBuffToUnit(buffPrefab, owner);
        }

        protected override string GetLogMessage()
        {
            return base.GetLogMessage() + " It makes the " + owner.info.unitName + " stronger in defence for " + buffPrefab.GetDuration() + " rounds!";
        }
    }
}