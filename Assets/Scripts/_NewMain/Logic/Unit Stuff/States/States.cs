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
        public bool IsInvulnerable()
        {
            return owner.buffs.FindAllBuffsOfType("Invulnerable").IsEmpty() == false;
        }

        //means can do shit
        public bool IsStunned()
        {
            return owner.buffs.FindAllBuffsOfType("Stunned").IsEmpty() == false;
        }

        //means cannot autoattack?
        public bool IsDisarmed()
        {
            return owner.buffs.FindAllBuffsOfType("Disarmed").IsEmpty() == false;
        }

        //means cannot retaliate
        public bool IsOverwhelmed()
        {
            return owner.buffs.FindAllBuffsOfType("Overwhelmed").IsEmpty() == false;
        }

        //means cannot fly
        public bool IsGrounded()
        {
            return owner.buffs.FindAllBuffsOfType("Grounded").IsEmpty() == false;
        }

        //means cannot move
        public bool IsImmobile()
        {
            return owner.buffs.FindAllBuffsOfType("Immobile").IsEmpty() == false;
        }

        // means cannot use abilities
        public bool IsSilenced()
        {
            return owner.buffs.FindAllBuffsOfType("Silenced").IsEmpty() == false;
        }

        //needs cool name like other buffs
        public bool IsPreventingRetaliation()
        {
            return owner.buffs.FindAllBuffsOfType("PreventingRetaliation").IsEmpty() == false;
        }

    }
}