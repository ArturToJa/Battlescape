using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattlescapeLogic
{
    public class ActiveDestroyObstacleAbility : AbstractActiveTileTargetAbility
    {

        protected override void Activate()
        {
            base.Activate();
            owner.statistics.numberOfAttacks--;
            Tile targetTile = target as Tile;
            targetTile.myObstacle.Destruct(owner);
        }

        public override bool IsLegalTarget(IMouseTargetable target)
        {
            if (base.IsLegalTarget(target) == false)
            {
                return false;
            }
            var targetTile = target as Tile;
            return targetTile.hasObstacle;
        }
    }
}