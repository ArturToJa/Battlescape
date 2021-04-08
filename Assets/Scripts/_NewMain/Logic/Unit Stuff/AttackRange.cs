using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattlescapeLogic
{
    public class AttackRange
    {
        int _baseCombatAttackRange;
        public int baseCombatAttackRange => _baseCombatAttackRange;

        public int bonusCombatAttackRange { get; set; }

        int _baseAttackRange;
        public int baseAttackRange => _baseAttackRange;

        public int bonusAttackRange { get; set; }

        int _minimalAttackRange;
        public int minimalAttackRange => _minimalAttackRange;

        public AttackRange(CSVData _data, string _myUnitName)
        {
            string[] data = _data.GetRightRow(_myUnitName);
            Dictionary<string, int> names = _data.names;

            int.TryParse(data[names["Combat Range"]], out _baseCombatAttackRange); 
            int.TryParse(data[names["Min Range"]], out _minimalAttackRange); 
            int.TryParse(data[names["Max Range"]], out _baseAttackRange);
        }

        public int GetCurrentAttackRangeOutOfCombat()
        {
            return baseAttackRange + bonusAttackRange;
        }

        public int GetCurrentAttackRangeInCombat()
        {
            return baseCombatAttackRange + bonusCombatAttackRange;
        }
    }
}