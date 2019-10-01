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
            //I wanted this to do OnHit, but it cannot give me info WHO is attacking so maybe it is not the correct way?
            //myUnit.OnHit()
        }
    }
}
