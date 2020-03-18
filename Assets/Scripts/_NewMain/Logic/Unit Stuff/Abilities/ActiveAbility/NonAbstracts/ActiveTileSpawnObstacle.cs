using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattlescapeLogic
{
    public class ActiveTileSpawnObstacle : AbstractActiveTileTargetAbility
    {

        [SerializeField] GameObject obstaclePrefab;

        public override bool IsLegalTarget(IMouseTargetable target)
        {
            if (target is Tile == false)
            {
                return false;
            }
            Tile targetTile = target as Tile;
            return IsInRange(targetTile)&& targetTile.IsWalkable();
        }

        protected override void Activate()
        {
            base.Activate();
            Tile targetTile = target as Tile;
            Obstacle obstacle = Instantiate(obstaclePrefab, targetTile.transform.position, obstaclePrefab.transform.rotation).GetComponent<Obstacle>();
            obstacle.OnSpawn(targetTile);
        }

    }
}