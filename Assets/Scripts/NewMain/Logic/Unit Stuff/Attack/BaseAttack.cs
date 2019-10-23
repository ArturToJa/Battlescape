using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BattlescapeLogic
{
    public enum AttackTypes
    {
        Melee = 0,
        Ranged = 1,
        Instant = 2
    }
    public class BaseAttack
    {
        public string name;
        protected Unit sourceUnit;
        protected Unit targetUnit;

        public BaseAttack(Unit _myUnit)
        {
            sourceUnit = _myUnit;
        }

        public virtual void Attack(Unit target)
        {
            //most likely:
            targetUnit = target;
            TurnTowardsTarget();
            PlayAttackAnimation();
        }
        protected virtual void PlayAttackAnimation()
        {
            sourceUnit.animator.SetTrigger("Attack");
        }

        //im aware im copying it from AbstractMovement ;/
        protected void TurnTowardsTarget()
        {
            sourceUnit.visuals.transform.LookAt(new Vector3(targetUnit.transform.position.x, sourceUnit.visuals.transform.position.y, targetUnit.transform.position.z));
        }

        public virtual void OnAttackAnimation()
        {
            //IDK if this needs to even be virtual because im not thinking anymore as it is very late at night when i'm coiding it.
            //Here we would calculate the damage.
            //IDK how much should be done here, and how much should be done on the unit's side (deal dmg vs get dmg)
            sourceUnit.HitTarget(targetUnit);
        }

        // Virtual function for Ranged Attack animations
        // It does nothing for basic attack
        public virtual void OnRangedAttackAnimation()
        {
        }
    }
}
