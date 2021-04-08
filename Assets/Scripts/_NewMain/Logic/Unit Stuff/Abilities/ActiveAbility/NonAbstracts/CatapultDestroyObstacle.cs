using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattlescapeLogic
{
    public class CatapultDestroyObstacle : AbstractDestroyObstacleAbility, IMissileLaucher
    {        
        protected override void AttackObstacle()
        {
            {
                Missile missile = Instantiate(owner.myMissile, owner.transform.position, owner.transform.rotation);
                //this should actually be SPAWNING POINT on shooter, not SHOOTER POSITION (not middle of a shooter lol)
                missile.sourceUnit = owner;
                missile.target = target.currentPosition.center;
                missile.myLauncher = this;
            }
        }

        public void OnMissileHitTarget()
        {
            Networking.instance.SendCommandToDestroyObstacle(target);
        }
    }
}