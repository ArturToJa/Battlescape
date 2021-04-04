using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattlescapeLogic
{
    public class Barricade : AbstractActiveTileTargetAbility
    {
        [SerializeField] DestructibleObstacle obstaclePrefab;
        

        public override void DoAbility()
        {
            DestructibleObstacle newObstacle = Instantiate(obstaclePrefab, target.transform.position, target.transform.rotation, target.transform);
            MultiTile position = MultiTile.Create(target, newObstacle.currentPosition.size);
            newObstacle.OnSpawn(position);
        }

        protected override string GetLogMessage()
        {
            return base.GetLogMessage() + " It creates an obstacle on a neighbouring tile.";
        }
    }
}