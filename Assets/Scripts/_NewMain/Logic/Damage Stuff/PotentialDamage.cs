using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattlescapeLogic
{
    public class PotentialDamage
    {
        public int avarageDamage { get; private set; }
        public int damageHalfRange { get; private set; }
        public float hitChance { get; private set; }

        public string GetText()
        {
            return GetHitMissChanceText() + "\n" + "\n" + GetDamageText();

        }

        string GetHitMissChanceText()
        {
            if (hitChance == 1)
            {
                return "Hit is certain.";
            }
            if (hitChance == 0)
            {
                return "Impossible to hit.";
            }
            else
            {
                return
                    "Chances for:" +
                    "\n" + "Miss (reducing Defence): " + ((1 - hitChance) * 100).ToString("F2") + "%" +
                    "\n" + "Hit (dealing Damage): " + (hitChance * 100).ToString("F2") + "%";
            }
        }

        string GetDamageText()
        {
            if (hitChance == 0)
            {
                return string.Empty;
            }
            if (damageHalfRange == 0)
            {
                return "Damage: " + avarageDamage.ToString();
            }
            else
            {
                return "Damage: " + (avarageDamage - damageHalfRange).ToString() + " - " + (avarageDamage + damageHalfRange).ToString();
            }    
        }

       
        

        public PotentialDamage(int _avarage, int _halfRange, float _chance)
        {
            avarageDamage = _avarage;
            damageHalfRange = _halfRange;
            hitChance = _chance;
        }
    }
}