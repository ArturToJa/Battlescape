using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattlescapeLogic
{
    public class ActiveShootingDestroyObstacleAbility : AbstractActiveObstacleTargetAbility, IMissileLaucher
    {
        protected override void Activate()
        {
            base.Activate();

            owner.statistics.numberOfAttacks--;

            Obstacle targetObstacle = target as Obstacle;

            Missile missile = GameObject.Instantiate(owner.myMissile, owner.transform.position, owner.transform.rotation).GetComponent<Missile>();
            //this should actually be SPAWNING POINT on shooter, not SHOOTER POSITION (not middle of a shooter lol)
            missile.sourceUnit = owner;
            missile.target = targetObstacle.currentPosition.center;
            missile.myLauncher = this;
        }

        public override bool IsLegalTarget(IMouseTargetable target)
        {
            if (base.IsLegalTarget(target))
            {
                var targetObstacle = target as Obstacle;
                return targetObstacle.currentPosition.width == 1 && targetObstacle.currentPosition.height == 1;
            }
            else
            {
                return false;
            }
        }

        public void OnMissileHitTarget()
        {
            Networking.instance.SendCommandToDestroyObstacle(owner, target as Obstacle);
        }
    }
}