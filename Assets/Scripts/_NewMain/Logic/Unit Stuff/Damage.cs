using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattlescapeLogic
{
    public class Damage
    {
        public Damage(int _baseDamage, bool _isHit)
        {
            baseDamage = _baseDamage;
            isHit = _isHit;
        }

        public int baseDamage { get; private set; }
        public bool isHit { get; private set; }
        public int bonusDamage = 0;
        public int bonusPercentDamage = 0;

        public int GetTotalDamage()
        {
            return baseDamage + bonusDamage + baseDamage * bonusPercentDamage / 100;
        }

        public static implicit operator int(Damage damage)
        {
            return damage.isHit ? damage.GetTotalDamage() : 0;
        }
    }
}