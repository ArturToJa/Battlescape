using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BattlescapeSound;


namespace BattlescapeLogic
{
    //this class has functions matching the events in unit model's animations.
    public class AnimationEvents : MonoBehaviour
    {
        Unit myUnit;

        void Start()
        {
            myUnit = transform.root.GetComponent<Unit>();
        }
       
        void Hit()
        {
            myUnit.attack.OnAttackAnimation();
            SoundManager.instance.PlaySound(myUnit.gameObject, myUnit.unitSounds.hitSound);
        }

        void Shoot()
        {
            myUnit.attack.OnRangedAttackAnimation();
            SoundManager.instance.PlaySound(myUnit.gameObject, myUnit.unitSounds.shootSound);
        }

        void FakeShoot()
        {
            SoundManager.instance.PlaySound(myUnit.gameObject, myUnit.unitSounds.shootSound);
        }
        void Step()
        {            
            SoundManager.instance.PlaySound(myUnit.gameObject, myUnit.unitSounds.movementSound);
        }

        void Swing()
        {
            SoundManager.instance.PlaySound(myUnit.gameObject, myUnit.unitSounds.attackSound);
        }

        void Death()
        {
            SoundManager.instance.PlaySound(myUnit.gameObject, myUnit.unitSounds.deathSound);
        }
    }
}
