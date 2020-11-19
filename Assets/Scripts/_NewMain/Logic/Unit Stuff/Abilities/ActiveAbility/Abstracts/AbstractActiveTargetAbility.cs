using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BattlescapeLogic
{
    public abstract class AbstractActiveTargetAbility : AbstractActiveAbility
    {
        public override void OnMouseHovered()
        { }
        public override void OnMouseUnHovered()
        { }

        public override void OnClickIcon()
        {
            base.OnClickIcon();
            ColourPossibleTargets();
        }
    }
}