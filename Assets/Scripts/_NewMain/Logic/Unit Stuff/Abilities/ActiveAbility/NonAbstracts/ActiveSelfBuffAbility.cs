using System.Collections.Generic;
using UnityEngine;

namespace BattlescapeLogic
{
    public class ActiveSelfBuffAbility : AbstractActiveAbility
    {
        [SerializeField] List<GameObject> selfBuffs;


        protected override void Activate()
        {
            base.Activate();
            ApplyBuffsToUnit(selfBuffs, owner);
        }

        public override void ColourPossibleTargets()
        {
            Debug.LogError("If you are asking this, most likely something went wrong?");
        }

        public override bool IsLegalTarget(IMouseTargetable target)
        {
            Debug.LogError("If you are asking this, most likely something went wrong?");
            return true;
        }


    }
}