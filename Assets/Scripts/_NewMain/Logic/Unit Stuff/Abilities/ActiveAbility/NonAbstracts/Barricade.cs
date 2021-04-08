using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattlescapeLogic
{
    public class Barricade : AbstractActiveMultiTileTargetAbility
    {
        [SerializeField] DestructibleObstacle obstaclePrefab;
        

        public override void DoAbility()
        {
            DestructibleObstacle newObstacle = Instantiate(obstaclePrefab, target.bottomLeftCorner.transform.position, target.bottomLeftCorner.transform.rotation, target.bottomLeftCorner.transform);
            newObstacle.OnSpawn(target);
        }

        protected override string GetLogMessage()
        {
            return base.GetLogMessage() + " It creates an obstacle on a neighbouring tile.";
        }
    }
}