using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BattlescapeLogic
{
    public class GuardianSupport : AbstractActiveUnitTargetAbility
    {
        [SerializeField] GameObject buffPrefab;

        public override void DoAbility()
        {
            ApplyBuffToUnit(buffPrefab, target);
        }

        protected override string GetLogMessage()
        {
            return base.GetLogMessage() + "on allied " + target + ", making him stronger!";
        }
    }
}