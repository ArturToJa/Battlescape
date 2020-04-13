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

            Missile missile = GameObject.Instantiate(owner.myMissile, owner.transform.position, owner.myMissile.transform.rotation).GetComponent<Missile>();
            missile.startingPoint = missile.transform.position;
            //this should actually be SPAWNING POINT on shooter, not SHOOTER POSITION (not middle of a shooter lol)
            missile.sourceUnit = owner;
            missile.target = targetObstacle.currentPosition[0];
            missile.myLauncher = this;
        }

        public override bool IsLegalTarget(IMouseTargetable target)
        {
            if (base.IsLegalTarget(target))
            {
                var targetObstacle = target as Obstacle;
                return targetObstacle.currentPosition.Length == 1;
            }
            else
            {
                return false;
            }
        }

        public void OnMissileHitTarget(Tile target)
        {
            NetworkingApiBaseClass.Instance.SendCommandToDestroyObstacle(owner, target.myObstacle);
        }
    }
}