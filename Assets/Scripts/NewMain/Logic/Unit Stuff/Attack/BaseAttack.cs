using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BattlescapeLogic
{
    public class BaseAttack
    {
        public string name;
        protected Unit myUnit;

        public BaseAttack(Unit _myUnit)
        {
            myUnit = _myUnit;
        }

        public virtual void Attack(Unit target)
        {
            //most likely:
            TurnTowards(target.transform.position);
            PlayAttackAnimation();
            DealDamageTo(target);
        }
        protected virtual void PlayAttackAnimation()
        {
            myUnit.animator.SetTrigger("Attack");
        }

        //im aware im copying it from AbstractMovement ;/
        protected void TurnTowards(Vector3 target)
        {
            myUnit.visuals.transform.LookAt(new Vector3(target.x, myUnit.visuals.transform.position.y, target.z));
        }

        protected virtual void DealDamageTo(Unit target)
        {
            //IDK if this needs to even be virtual because im not thinking anymore as it is very late at night when i'm coiding it.
            //Here we would calculate the damage.
            //IDK how much should be done here, and how much should be done on the unit's side (deal dmg vs get dmg)
            int amount = 0;
            target.statistics.healthPoints -= amount;
        }
    }
}
