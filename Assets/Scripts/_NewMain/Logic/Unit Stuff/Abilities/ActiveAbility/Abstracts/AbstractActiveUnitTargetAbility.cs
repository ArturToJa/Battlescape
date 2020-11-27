using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattlescapeLogic
{
    public abstract class AbstractActiveUnitTargetAbility : AbstractActiveTargetAbility
    {
        protected Unit target;

        public override void ColourPossibleTargets()
        {
            foreach (Unit unit in Global.instance.GetAllUnits())
            {
                if (CheckTarget(unit))
                {
                    BattlescapeGraphics.ColouringTool.ColourObject(unit, targetColouringColour);
                }
            }
        }

        public override void OnLeftClick(IMouseTargetable clickedObject, Vector3 exactClickPosition)
        {
            if (CheckTarget(clickedObject))
            {
                target = clickedObject as Unit;
                Activate();
            }
        }

        protected override bool IsLegalTarget(IMouseTargetable target)
        {
            return base.IsLegalTarget(target) && target as Unit != null;
        } 
    }
}