using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattlescapeLogic
{
    public abstract class AbstractActiveDestructibleTargetAbility : AbstractActiveTargetAbility
    {
        protected DestructibleObstacle target;

        public override void ColourPossibleTargets()
        {
            foreach (DestructibleObstacle destructible in Global.instance.GetAllDestructibles())
            {
                if (IsLegalTarget(destructible))
                {
                    BattlescapeGraphics.ColouringTool.ColourObject(destructible, Color.green);
                }
            }
        }

        public override void OnLeftClick(IMouseTargetable clickedObject, Vector3 exactClickPosition)
        {
            if (IsLegalTarget(clickedObject))
            {
                target = clickedObject as DestructibleObstacle;
                Activate(clickedObject);
            }
        }

        protected override bool IsLegalTarget(IMouseTargetable target)
        {
            return base.IsLegalTarget(target) && target as DestructibleObstacle != null;
        }
    }
}
