using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattlescapeLogic
{
    public abstract class AbstractActiveObstacleTargetAbility : AbstractActiveTargetAbility
    {

        protected Obstacle target;

        public override void ColourPossibleTargets()
        {
            foreach (Obstacle obstacle in Global.instance.GetAllObstacles())
            {
                if (IsLegalTarget(obstacle))
                {
                    BattlescapeGraphics.ColouringTool.ColourObject(obstacle.currentPosition, GetColourForTargets());
                }
            }
        }

        public override void OnLeftClick(IMouseTargetable clickedObject, Vector3 exactClickPosition)
        {
            if (IsLegalTarget(clickedObject))
            {
                target = clickedObject as Obstacle;
                Activate(clickedObject);
            }
        }

        protected override bool IsLegalTarget(IMouseTargetable target)
        {
            return base.IsLegalTarget(target) && target as Obstacle != null;
        }

    }
}