using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BattlescapeLogic
{
    //this class has functions matching the events in unit model's animations.
    public class AnimationEvents : MonoBehaviour
    {
        Unit myUnit;

        void Start()
        {
            myUnit = transform.parent.GetComponent<Unit>();
        }

       
        public void Hit()
        {
            myUnit.attack.OnAttackAnimation();
        }

        public void Shoot()
        {
            myUnit.attack.OnRangedAttackAnimation();
        }
    }
}
