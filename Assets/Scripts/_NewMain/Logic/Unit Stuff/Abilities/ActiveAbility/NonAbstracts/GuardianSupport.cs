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
            ApplyBuffToUnit(buffPrefab, target as Unit);
        }
    }
}