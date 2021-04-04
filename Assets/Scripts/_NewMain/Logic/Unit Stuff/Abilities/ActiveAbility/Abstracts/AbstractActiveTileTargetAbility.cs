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
                    BattlescapeGraphics.ColouringTool.ColourObject(tile, GetColourForTargets());
                }
            }
        }

        public override void OnLeftClick(IMouseTargetable clickedObject, Vector3 exactClickPosition)
        {
            if (IsLegalTarget(clickedObject))
            {
                target = clickedObject as Tile;
                Activate(clickedObject);
            }
        }

        protected override bool IsLegalTarget(IMouseTargetable target)
        {
            if (target is Tile == false)
            {
                return false;
            }
            var targetTile = target as Tile;
            return this.owner.currentPosition.DistanceTo(targetTile) <= range;
        }

        public override Color GetColourForTargets()
        {
            return Global.instance.colours.blue;
        }
    }
}