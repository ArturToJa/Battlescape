using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattlescapeLogic
{
    public abstract class AbstractActiveTileTargetAbility : AbstractActiveTargetAbility
    {
        protected Tile target;

        public override void ColourPossibleTargets()
        {
            foreach (Tile tile in Global.instance.currentMap.board)
            {
                if (CheckTarget(tile))
                {
                    BattlescapeGraphics.ColouringTool.ColourObject(tile, Color.green);
                }
            }
        }

        public override void OnLeftClick(IMouseTargetable clickedObject, Vector3 exactClickPosition)
        {
            if (IsLegalTarget(clickedObject))
            {
                target = clickedObject as Tile;
                Activate();
            }
        }

        protected override bool IsLegalTarget(IMouseTargetable target)
        {
            return base.IsLegalTarget(target) && target as Tile != null;
        }
    }
}