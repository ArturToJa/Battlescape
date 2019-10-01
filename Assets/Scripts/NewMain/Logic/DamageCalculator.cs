using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BattlescapeLogic
{
    public class DamageCalculator
    {
        static DamageCalculator _instance;
        public static DamageCalculator instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new DamageCalculator();
                }
                return _instance;
            }
        }
        public int CalculateDamage(Unit source, Unit target)
        {
            return 0;
        }
    }
}