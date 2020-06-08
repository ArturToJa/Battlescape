using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattlescapeLogic
{
    public class ActiveTileSpawnObstacle : AbstractActiveAbility
    {
        /// <summary>
        /// Fix needed - isLegalTarget creates MultiTile based on height and with of owner, 
        /// should do it based on Obstacle height and width
        /// </summary>


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


        public override bool IsLegalTarget(IMouseTargetable target, Vector3 exactClickPosition)
        {
            if (target is Tile == false)
            {
                return false;
            }
            Tile targetTile = target as Tile;
            MultiTile position = targetTile.PositionRelatedToMouse(owner.currentPosition.width, owner.currentPosition.height, exactClickPosition);
            return IsInRange(position) && targetTile.IsWalkable();
        }

        public override void ColourPossibleTargets()
        {
            foreach (Tile tile in Global.instance.currentMap.board)
            {
                if (IsLegalTarget(tile, Vector3.zero))
                {
                    tile.highlighter.TurnOn(targetColouringColour);
                }
            }
        }

    }
}