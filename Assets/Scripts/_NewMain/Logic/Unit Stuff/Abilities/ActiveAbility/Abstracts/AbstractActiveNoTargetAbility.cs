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
            Activate(null);
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

        public override void OnLeftClick(IMouseTargetable clickedObject, Vector3 exactClickPosition)
        {
            //worth mentionning that this will literally never occur (you would have to click something between clicking this ability icon AND this ability resolving but it resolves immidiately on clicking its icon)
            return;
        }

        public override Color GetColourForTargets()
        {
            return Color.white; //will never be used
        }
    }
}