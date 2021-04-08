using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattlescapeLogic
{
    [System.Serializable]
    public class ChangeableStatistics
    {
        [SerializeField] int _bonusAttack;
        public int bonusAttack => _bonusAttack;
        [SerializeField] int _bonusAttackPercent;
        public int bonusAttackPercent => _bonusAttackPercent;

        [SerializeField] int _bonusMeleeProficiency;
        public int bonusMeleeProficiency => _bonusMeleeProficiency;

        [SerializeField] int _bonusMeleeProficiencyPercent;
        public int bonusMeleeProficiencyPercent => _bonusMeleeProficiencyPercent;

        [SerializeField] int _bonusDefence;
        public int bonusDefence => _bonusDefence;

        [SerializeField] int _bonusDefencePercent;
        public int bonusDefencePercent => _bonusDefencePercent;

        [SerializeField] int _bonusHealth;
        public int bonusHealth => _bonusHealth;

        [SerializeField] int _bonusHealthPercent;
        public int bonusHealthPercent => _bonusHealthPercent;

        [SerializeField] int _bonusMovementPoints;
        public int bonusMovementPoints => _bonusMovementPoints;

        [SerializeField] int _bonusMovementPointsPercent;
        public int bonusMovementPointsPercent => _bonusMovementPointsPercent;

        [SerializeField] int _bonusAttackRange;
        public int bonusAttackRange => _bonusAttackRange;

        [SerializeField] int _bonusAttackRangePercent;
        public int bonusAttackRangePercent => _bonusAttackRangePercent;

        [SerializeField] int _bonusCombatAttackRange;
        public int bonusCombatAttackRange => _bonusCombatAttackRange;

        [SerializeField] int _bonusCombatAttackRangePercent;
        public int bonusCombatAttackRangePercent => _bonusCombatAttackRangePercent;

        [SerializeField] int _bonusNumberOfAttacks;
        public int bonusNumberOfAttacks => _bonusNumberOfAttacks;

        [SerializeField] int _bonusNumberOfAttacksPercent;
        public int bonusNumberOfAttacksPercent => _bonusNumberOfAttacksPercent;

        [SerializeField] int _bonusNumberOfRetaliations;
        public int bonusNumberOfRetaliations => _bonusNumberOfRetaliations;

        [SerializeField] int _bonusNumberOfRetaliationsPercent;
        public int bonusNumberOfRetaliationsPercent => _bonusNumberOfRetaliationsPercent;

        [SerializeField] int _bonusEnergyRegen;
        public int bonusEnergyRegen => _bonusEnergyRegen;

        [SerializeField] int _bonusEnergyRegenPercent;
        public int bonusEnergyRegenPercent => _bonusEnergyRegenPercent;



        public void SetPercentages(Statistics statistics)
        {
            _bonusAttack += (statistics.baseAttack + statistics.bonusAttack) * _bonusAttackPercent / 100;
            _bonusAttackRange += (statistics.attackRange.baseAttackRange + statistics.attackRange.bonusAttackRange) * _bonusAttackRangePercent / 100;
            _bonusCombatAttackRange += (statistics.attackRange.baseCombatAttackRange + statistics.attackRange.bonusCombatAttackRange) * _bonusCombatAttackRangePercent / 100;
            _bonusDefence += (statistics.GetCurrentDefence()) * _bonusDefencePercent / 100;
            _bonusEnergyRegen += (statistics.energy.GetCurrentRegen()) * _bonusEnergyRegenPercent / 100;
            _bonusHealth += (statistics.healthPoints) * _bonusHealthPercent / 100;
            _bonusMeleeProficiency += (statistics.GetCurrentMeleeProficiency()) * _bonusMeleeProficiencyPercent / 100;
            _bonusMovementPoints += (statistics.GetCurrentMaxMovementPoints()) * _bonusMovementPointsPercent/ 100;
            _bonusNumberOfAttacks += (statistics.GetCurrentMaxNumberOfAttacks()) * _bonusNumberOfAttacksPercent / 100;
            _bonusNumberOfRetaliations += (statistics.GetCurrentMaxNumberOfRetaliations()) * _bonusNumberOfRetaliationsPercent/ 100;
        }
    }
}

