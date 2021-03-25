using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattlescapeLogic
{
    public class Damage
    {
        public Damage(int _baseDamage, bool _isHit, bool _stopsRetaliation)
        {
            baseDamage = _baseDamage;
            isHit = _isHit;
            stopsRetaliation = _stopsRetaliation;
        }

        public int baseDamage { get; private set; }
        public bool isHit { get; private set; }
        public int bonusDamage = 0;
        public int bonusPercentDamage = 0;
        public bool stopsRetaliation;

        public int GetTotalDamage()
        {
            return baseDamage + bonusDamage + baseDamage * bonusPercentDamage / 100;
        }

        public static implicit operator int(Damage damage)
        {
            return damage.isHit ? damage.GetTotalDamage() : 0;
        }

        public static implicit operator string(Damage damage)
        {
            return (damage.isHit ? damage.GetTotalDamage() : 0).ToString();
        }
    }
}