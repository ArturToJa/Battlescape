using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattlescapeLogic
{
    public class Statistics
    {
        public const int baseDamage = 100;
        
        int _cost;
        public int cost => _cost;
        
        int _limit;
        public int limit => _limit;        

        int _baseAttack;
        public int baseAttack => _baseAttack;
        
        public int bonusAttack { get; set; }

        int _baseMeleeProficiency;
        public int baseMeleeProficiency => _baseMeleeProficiency;

        public int bonusMeleeProficiency { get; set; }

        int _baseDefence;
        public int baseDefence => _baseDefence;        
        
        public int bonusDefence { get; set; }

        int _baseMaxHealthPoints;
        public int baseMaxHealthPoints => _baseMaxHealthPoints;
        
        public int bonusMaxHealthPoints { get; set; }

        public int healthPoints { get; set; }

        int _baseMaxMovementPoints;
        public int baseMaxMovementPoints => _baseMaxMovementPoints;        
        public int bonusMaxMovementPoints { get; set; }
        public int movementPoints { get; set; }              

        int _baseMaxNumberOfAttacks;
        public int baseMaxNumberOfAttacks => _baseMaxNumberOfAttacks;        

        public int bonusMaxNumberOfAttacks { get; set; }

        public int numberOfAttacks { get; set; }

        int _baseMaxNumberOfRetaliations;
        public int baseMaxNumberOfRetaliations => _baseMaxNumberOfRetaliations;

        public int bonusMaxNumberOfRetaliations { get; set; }

        public int numberOfRetaliations { get; set; }  
        
        public Energy energy { get; private set; }

        public AttackRange attackRange { get; set; }

        public Statistics(CSVData _data, string _myUnitName)
        {
            string[] data = _data.GetRightRow(_myUnitName);
            Dictionary<string, int> names = _data.names;
            
            int.TryParse(data[names["Attack"]], out _baseAttack);
            int.TryParse(data[names["Defence"]], out _baseDefence);
            int.TryParse(data[names["HP"]], out _baseMaxHealthPoints);
            int.TryParse(data[names["MS"]], out _baseMaxMovementPoints);
            int.TryParse(data[names["Attacks/Turn"]], out _baseMaxNumberOfAttacks);
            int.TryParse(data[names["Retaliations/Turn"]], out _baseMaxNumberOfRetaliations);
            int.TryParse(data[names["Cost"]], out _cost);
            int.TryParse(data[names["Limitation"]], out _limit);
            int.TryParse(data[names["Melee Proficiency"]], out _baseMeleeProficiency);
            energy = new Energy();
            attackRange = new AttackRange(_data, _myUnitName);
        }

        public void NullMaxNumberOfAttacks()
        {
            _baseMaxNumberOfAttacks = 0;
        }

        public void NullMaxMovementPoints()
        {
            _baseMaxMovementPoints = 0;
        }

        public void NullBaseAttack()
        {
            _baseAttack = 0;
        }

        public int GetCurrentRangeAttack()
        {
            return baseAttack + bonusAttack;
        }

        public int GetCurrentMeleeAttack()
        {
            return Mathf.RoundToInt((baseAttack + bonusAttack) * GetCurrentMeleeProficiency() / 100);
        }

        public int GetCurrentMeleeProficiency()
        {
            return baseMeleeProficiency + bonusMeleeProficiency;
        }
        public int GetCurrentDefence()
        {
            return baseDefence + bonusDefence;
        }
        
        public int GetCurrentMaxMovementPoints()
        {
            return baseMaxMovementPoints + bonusMaxMovementPoints;
        }

        public int GetCurrentMaxNumberOfRetaliations()
        {
            return baseMaxNumberOfRetaliations + bonusMaxNumberOfRetaliations;
        }

        public int GetCurrentMaxHealtPoints()
        {
            return baseMaxHealthPoints + bonusMaxHealthPoints;
        }

        public int GetCurrentMaxNumberOfAttacks()
        {
            return baseMaxNumberOfAttacks + bonusMaxNumberOfAttacks;
        }       

        public void ApplyBonusStatistics(ChangeableStatistics bonusStatistics)
        {
            //FIrst set bonusStatistics to bigger values (depending on percentages) - NOTE it means that if bonusStatistics have BOTH flat and percent values, flats dont get percenteged.
            bonusStatistics.SetPercentages(this);
            // statistics increases in non-dumb way - we don't want to increase base statistics, only bonus ones
            bonusAttack += bonusStatistics.bonusAttack;
            bonusMeleeProficiency += bonusStatistics.bonusMeleeProficiency;
            bonusDefence += bonusStatistics.bonusDefence;
            attackRange.bonusAttackRange += bonusStatistics.bonusAttackRange;
            attackRange.bonusCombatAttackRange += bonusStatistics.bonusCombatAttackRange;
            bonusMaxHealthPoints += bonusStatistics.bonusHealth;
            healthPoints += bonusStatistics.bonusHealth;
            bonusMaxMovementPoints += bonusStatistics.bonusMovementPoints;
            movementPoints += bonusStatistics.bonusMovementPoints;
            bonusMaxNumberOfAttacks += bonusStatistics.bonusNumberOfAttacks;
            numberOfAttacks += bonusStatistics.bonusNumberOfAttacks;
            bonusMaxNumberOfRetaliations += bonusStatistics.bonusNumberOfRetaliations;
            numberOfRetaliations += bonusStatistics.bonusNumberOfRetaliations;
            energy.bonusRegen += bonusStatistics.bonusEnergyRegen;
        }

        public void RemoveBonusStatistics(ChangeableStatistics bonusStatistics)
        {           
            bonusAttack -= bonusStatistics.bonusAttack;
            bonusMeleeProficiency -= bonusStatistics.bonusMeleeProficiency;
            bonusDefence -= bonusStatistics.bonusDefence;
            attackRange.bonusAttackRange -= bonusStatistics.bonusAttackRange;
            attackRange.bonusCombatAttackRange -= bonusStatistics.bonusCombatAttackRange;
            bonusMaxHealthPoints -= bonusStatistics.bonusHealth;
            if (healthPoints > GetCurrentMaxHealtPoints())
            {
                healthPoints = GetCurrentMaxHealtPoints();
            }
            bonusMaxNumberOfAttacks -= bonusStatistics.bonusNumberOfAttacks;
            if (numberOfAttacks > GetCurrentMaxNumberOfAttacks())
            {
                numberOfAttacks = GetCurrentMaxNumberOfAttacks();
            }
            bonusMaxNumberOfRetaliations -= bonusStatistics.bonusNumberOfRetaliations;
            if (numberOfRetaliations > GetCurrentMaxNumberOfRetaliations())
            {
                numberOfRetaliations = GetCurrentMaxNumberOfRetaliations();
            }           
            bonusMaxMovementPoints -= bonusStatistics.bonusMovementPoints;
            if (movementPoints > GetCurrentMaxMovementPoints())
            {
                movementPoints = GetCurrentMaxMovementPoints();
            }
            energy.bonusRegen -= bonusStatistics.bonusEnergyRegen;
        }

        public void OnOwnerTurn()
        {
            movementPoints = GetCurrentMaxMovementPoints();
            numberOfAttacks = GetCurrentMaxNumberOfAttacks();
            numberOfRetaliations = GetCurrentMaxNumberOfRetaliations();
            energy.OnOwnerTurn();
        }
    }
}
