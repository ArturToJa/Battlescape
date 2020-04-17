using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattlescapeLogic
{
    public class ActiveShootingDestroyObstacleAbility : AbstractActiveAbility, IMissileLaucher
    {
        protected override void Activate()
        {
            base.Activate();

            owner.statistics.numberOfAttacks--;

            Obstacle targetObstacle = target as Obstacle;

            Missile missile = GameObject.Instantiate(owner.myMissile, owner.transform.position, owner.myMissile.transform.rotation).GetComponent<Missile>();
            missile.startingPoint = missile.transform.position;
            //this should actually be SPAWNING POINT on shooter, not SHOOTER POSITION (not middle of a shooter lol)
            missile.sourceUnit = owner;
            missile.target = targetObstacle.currentPosition[0];
            missile.myLauncher = this;
        }
        
        public void OnMissileHitTarget(Tile target)
        {
            Networking.instance.SendCommandToDestroyObstacle(owner, target.myObstacle);
        }

        public override bool IsLegalTarget(IMouseTargetable target)
        {
            if (target is Obstacle == false)
            {
                return false;
            }
            Obstacle targetObstacle = target as Obstacle;
            return IsInRange(targetObstacle) && filter.FilterObstacle(targetObstacle) && targetObstacle.currentPosition.Length == 1;
        }

        public override void ColourPossibleTargets()
        {
            foreach (Tile tile in Global.instance.currentMap.board)
            {
                if (IsLegalTarget(tile.myObstacle))
                {
                    tile.highlighter.TurnOn(targetColouringColour);
                }
            }
        }
    }

}
