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

        protected override void Activate(IMouseTargetable target)
        {
            owner.movement.ApplyUnit(owner);
            owner.movement.TurnTowards((target as MonoBehaviour).transform.position);
            base.Activate(target);
        }
    }
}