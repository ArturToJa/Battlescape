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
            //this should actually be SPAWNING POINT on shooter, not SHOOTER POSITION (not middle of a shooter lol)
            missile.sourceUnit = owner;
            missile.target = targetObstacle.currentPosition.center;
            missile.myLauncher = this;
        }

        public override bool IsLegalTarget(IMouseTargetable target, Vector3 exactClickPosition)
        {
            if (target is Obstacle == false)
            {
                return false;
            }
            Obstacle targetObstacle = target as Obstacle;
            return IsInRange(targetObstacle) && filter.FilterObstacle(targetObstacle) && targetObstacle.currentPosition.Size() == 1;
        }

        public override void ColourPossibleTargets()
        {
            foreach(Obstacle obstalce in Global.instance.currentMap.obstacles)
            {
                if (IsLegalTarget(obstalce))
                {
                    foreach(Tile tile in obstalce.currentPosition)
                    {
                        tile.highlighter.TurnOn(targetColouringColour);
                    }
                }
            }
        }

        public void OnMissileHitTarget()
        {
            Networking.instance.SendCommandToDestroyObstacle(owner, target as Obstacle);
        }
    }

}
