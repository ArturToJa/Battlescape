using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattlescapeLogic
{
    public class ActiveTileSpawnObstacle : AbstractActiveAbility
    {
        [SerializeField] GameObject obstaclePrefab;


        protected override void Activate()
        {
            base.Activate();            
        }

        public override void OnAnimationEvent()
        {
            SpawnObstacle();
        }

        void SpawnObstacle()
        {
            Tile targetTile = target as Tile;
            Obstacle obstacle = Instantiate(obstaclePrefab, targetTile.transform.position, obstaclePrefab.transform.rotation).GetComponent<Obstacle>();
            obstacle.OnSpawn(targetTile);
        }


        public override bool IsLegalTarget(IMouseTargetable target)
        {
            if (target is Tile == false)
            {
                return false;
            }
            Tile targetTile = target as Tile;
            return IsInRange(targetTile) && targetTile.IsWalkable();
        }

        public override void ColourPossibleTargets()
        {
            foreach (Tile tile in Global.instance.currentMap.board)
            {
                if (IsLegalTarget(tile))
                {
                    tile.highlighter.TurnOn(targetColouringColour);
                }
            }
        }

    }
}