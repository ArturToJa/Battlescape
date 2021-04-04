using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattlescapeLogic
{
    public class LivingWall : AbstractActiveNoTargetAbility
    {
        [SerializeField] AbstractBuff buffPrefab;


        public override void DoAbility()
        {
            ApplyBuffToUnit(buffPrefab, owner);
        }

        protected override string GetLogMessage()
        {
            return base.GetLogMessage() + " It makes the " + owner.info.unitName + " immune to damage for " + buffPrefab.GetDuration() + " rounds!";
        }

    }
}