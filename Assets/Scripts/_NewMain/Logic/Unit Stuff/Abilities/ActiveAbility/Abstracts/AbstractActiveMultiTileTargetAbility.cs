using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattlescapeLogic
{
    public abstract class AbstractActiveMultiTileTargetAbility : AbstractActiveTargetAbility
    {
        protected MultiTile target;
        [SerializeField] Size targetSize;

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
                target = (clickedObject as Tile).PositionRelatedToMouse(targetSize,exactClickPosition);
                Activate(clickedObject);
            }
        }

        protected override bool IsLegalTarget(IMouseTargetable target)
        {
            var tileTarget = target as Tile;
            if (tileTarget == null)
            {
                return false;
            }
            MultiTile multiTileTarget = MultiTile.Create(tileTarget, targetSize);
            return this.owner.currentPosition.DistanceTo(multiTileTarget) <= range;
        }

        public override Color GetColourForTargets()
        {
            return Global.instance.colours.blue;
        }
    }
}