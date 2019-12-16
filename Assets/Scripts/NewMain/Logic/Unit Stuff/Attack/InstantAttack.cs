using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BattlescapeLogic
{
    public class InstantAttack : AbstractAttack
    {
        public InstantAttack(Unit _myUnit) : base(_myUnit)
        {
        }

        public override void Attack(Unit target)
        {
            base.Attack(target);
            PlayAttackAnimation();
        }

        protected override void PlayAttackAnimation()
        {
            //Most likely
            sourceUnit.animator.SetTrigger("Cast");

        }

        public override void OnAttackAnimation()
        {
            //IDK if this needs to even be virtual because im not thinking anymore as it is very late at night when i'm coiding it.
            //Here we would calculate the damage.
            //IDK how much should be done here, and how much should be done on the unit's side (deal dmg vs get dmg)
            sourceUnit.HitTarget(targetUnit);
        }

        public override void OnRangedAttackAnimation()
        {
        }
    }
}
