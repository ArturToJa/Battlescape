using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BattlescapeLogic
{
    public class GuardianSupport : AbstractActiveUnitTargetAbility
    {
        [SerializeField] StatisticChangeBuff buffPrefab;

        public override void DoAbility()
        {
            ApplyBuffToUnit(buffPrefab, target);
        }

        public override Color GetColourForTargets()
        {
            return Global.instance.colours.green;
        }

        protected override string GetLogMessage()
        {
            return base.GetLogMessage() + "\n" + "Allied " + target.info.unitName + " is stronger now (+" + buffPrefab.statistics.bonusAttack.ToString() + " attack, +" + buffPrefab.statistics.bonusDefence.ToString() + " defence)!";
        }
    }
}