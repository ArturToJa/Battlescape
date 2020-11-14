using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BattlescapeLogic
{
    public class GuardianSupport : AbstractActiveTargetAbility
    {
        [SerializeField] GameObject buffPrefab;

        public override void OnAnimationEvent()
        {
            ApplyBuffToUnit(buffPrefab, target as Unit);
        }
    }
}