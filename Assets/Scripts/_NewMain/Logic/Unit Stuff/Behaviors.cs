using System;
using UnityEngine;
namespace BattlescapeLogic
{
    public class Behaviors
    {
        private Unit owner;

        public Behaviors(Unit _owner)
        {
            owner = _owner;
        }

        public bool isUndamageable
        {
            get
            {
                return owner.buffs.FindAllBuffsOfType("Undamageable").IsEmpty() == false;
            }
        }
        
        public bool isStunned
        {
            get
            {
                return owner.buffs.FindAllBuffsOfType("Stunn").IsEmpty() == false;
            }
        }
        
        public bool cantRetaliate
        {
            get
            {
                return owner.buffs.FindAllBuffsOfType("Settle").IsEmpty() == false || isStunned;
            }
        }

        public bool isGrounded
        {
            get
            {
                return owner.buffs.FindAllBuffsOfType("Grounded").IsEmpty() == false || isStunned;
            }
        }

        public bool isDisarmed
        {
            get
            {
                return owner.buffs.FindAllBuffsOfType("Disarm").IsEmpty() == false || isStunned;
            }
        }

        public bool isSilenced
        {
            get
            {
                return owner.buffs.FindAllBuffsOfType("Silence").IsEmpty() == false || isStunned;
            }
        }
    }
}
