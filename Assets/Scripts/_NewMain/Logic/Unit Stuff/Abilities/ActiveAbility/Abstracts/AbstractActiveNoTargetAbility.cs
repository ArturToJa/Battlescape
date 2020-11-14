using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattlescapeLogic
{
    public abstract class AbstractActiveNoTargetAbility : AbstractActiveAbility
    {
        public override void OnClickIcon()
        {
            base.OnClickIcon();
            Activate();
        }

        public override void ColourPossibleTargets()
        {}

        protected override bool IsLegalTarget(IMouseTargetable target)
        {
            return true;
        }

        public override void OnAnimationEvent()
        {}
    }
}