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

        public override void OnMouseHovered()
        {
            ColourPossibleTargets();
        }
        public override void OnMouseUnHovered()
        {
            BattlescapeGraphics.ColouringTool.UncolourAllObjects();
        }

        public override void ColourPossibleTargets()
        {
            foreach (Unit unit in Global.instance.GetAllUnits())
            {
                if (CheckTarget(unit))
                {
                    BattlescapeGraphics.ColouringTool.ColourObject(unit, Color.green);
                }
            }
        }
    }
}