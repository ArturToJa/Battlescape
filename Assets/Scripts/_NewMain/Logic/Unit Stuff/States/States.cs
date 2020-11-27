using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattlescapeLogic
{
    public class States
    {
        Unit owner;

        public States(Unit _owner)
        {
            owner = _owner;
        }

        //means cannot receive dmg
        public bool isInvulnerable()
        {
            return owner.buffs.FindAllBuffsOfType("Invulnerable").IsEmpty() == false;
        }

        //means can do shit
        public bool isStunned()
        {
            return owner.buffs.FindAllBuffsOfType("Stunned").IsEmpty() == false;
        }

        public bool isDisarmed()
        {
            return owner.buffs.FindAllBuffsOfType("Disarmed").IsEmpty() == false;
        }

        //means cannot retaliate
        public bool isOverwhelmed()
        {
            return owner.buffs.FindAllBuffsOfType("Overwhelmed").IsEmpty() == false;
        }

        //means cannot fly
        public bool isGrounded()
        {
            return owner.buffs.FindAllBuffsOfType("Grounded").IsEmpty() == false;
        }

        //means cannot move
        public bool isImmobile()
        {
            return owner.buffs.FindAllBuffsOfType("Immobile").IsEmpty() == false;
        }

        // means cannot use abilities
        public bool isSilenced()
        {
            return owner.buffs.FindAllBuffsOfType("Silenced").IsEmpty() == false;
        }
    }
}