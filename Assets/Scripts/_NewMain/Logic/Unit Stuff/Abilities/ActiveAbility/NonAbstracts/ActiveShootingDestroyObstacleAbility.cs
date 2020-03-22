using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattlescapeLogic
{
    public class ActiveShootingDestroyObstacleAbility : AbstractActiveObstacleTargetAbility
    {
        [SerializeField] GameObject missilePrefab;


        protected override void Activate()
        {
            base.Activate();

            owner.statistics.numberOfAttacks--;

            Obstacle targetObstacle = target as Obstacle;

            ObstacleMissile missile = GameObject.Instantiate(missilePrefab, owner.transform.position, missilePrefab.transform.rotation).GetComponent<ObstacleMissile>();
            missile.startingPoint = missile.transform.position;
            //this should actually be SPAWNING POINT on shooter, not SHOOTER POSITION (not middle of a shooter lol)
            missile.sourceUnit = owner;
            missile.target = targetObstacle.currentPosition[0];
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


    }
}