using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BattlescapeLogic
{
    public class InstantAttack : BaseAttack
    {
        public InstantAttack(Unit _myUnit) : base(_myUnit)
        {
        }

        public override void Attack(Unit target)
        {
            
        }

        protected override void PlayAttackAnimation()
        {
            //Most likely
            myUnit.animator.SetTrigger("Cast");

        }
    }
}
