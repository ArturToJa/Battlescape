using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattlescapeLogic
{
    [System.Serializable]
    public class Statistics
    {
        public static readonly int baseDamage = 10;
        public static readonly int maxEnergy = 100;

        [SerializeField] int _cost;
        public int cost
        {
            get
            {
                return _cost;
            }
            set
            {
                _cost = value;
            }
        }

        [SerializeField] int _limit;
        public int limit
        {
            get
            {
                return _limit;
            }
            private set
            {
                _limit = value;
            }
        }

        [SerializeField] int _baseAttack;
        public int baseAttack
        {
            get
            {
                return _baseAttack;
            }
            private set
            {
                _baseAttack = value;
            }
        }
        public int bonusAttack { get; set; }

        [SerializeField] int _baseDefence;
        public int baseDefence
        {
            get
            {
                return _baseDefence;
            }
            private set
            {
                _baseDefence = value;
            }
        }
        public int bonusDefence { get; set; }
        [SerializeField] int _maxHealthPoints;
        public int maxHealthPoints
        {
            get
            {
                return _maxHealthPoints;
            }
            private set
            {
                _maxHealthPoints = value;
            }
        }
        public int healthPoints { get; set; }
        [SerializeField] int _baseMaxMovementPoints;
        public int baseMaxMovementPoints
        {
            get
            {
                return _baseMaxMovementPoints;
            }
            private set
            {
                _baseMaxMovementPoints = value;
            }
        }
        public int bonusMaxMovementPoints { get; set; }
        public int movementPoints { get; set; }

        [SerializeField] int _baseAttackRange = 1;
        public int baseAttackRange
        {
            get
            {
                return _baseAttackRange;
            }
            private set
            {
                _baseAttackRange = value;
            }
        }
        public int bonusAttackRange { get; set; }

        [SerializeField] int _minimalAttackRange = 0;
        public int minimalAttackRange
        {
            get
            {
                return _minimalAttackRange;
            }
            private set
            {
                _minimalAttackRange = value;
            }
        }

        [SerializeField] int _maxNumberOfAttacks = 1;
        public int maxNumberOfAttacks
        {
            get
            {
                return _maxNumberOfAttacks;
            }
            private set
            {
                _maxNumberOfAttacks = value;
            }
        }
        public int numberOfAttacks { get; set; }

        [SerializeField] int _defaultMaxNumberOfRetaliations;

        public int defaultMaxNumberOfRetaliations
        {
            get
            {
                return _defaultMaxNumberOfRetaliations;
            }
            private set
            {
                _defaultMaxNumberOfRetaliations = value;
            }
        }

        public int currentMaxNumberOfRetaliations { get; set; }

        public int numberOfRetaliations { get; set; }   

        public int currentEnergy { get; set; }
        [SerializeField] int _energyRegen;
        public int energyRegen
        {
            get
            {
                return _energyRegen;
            }
            private set
            {
                _energyRegen = value;
            }
        }

        public void NullMaxNumberOfAttacks()
        {
            maxNumberOfAttacks = 0;
        }

        public void NullMaxMovementPoints()
        {
            _baseMaxMovementPoints = 0;
        }

        public void NullBaseAttack()
        {
            _baseAttack = 0;
        }

        public int GetCurrentAttack()
        {
            return baseAttack + bonusAttack;
        }
        public int GetCurrentDefence()
        {
            return baseDefence + bonusDefence;
        }
        public int GetCurrentAttackRange()
        {
            return baseAttackRange + bonusAttackRange;
        }
        public int GetCurrentMaxMovementPoints()
        {
            return baseMaxMovementPoints + bonusMaxMovementPoints;
        }

        public void ApplyBonusStatistics(ChangeableStatistics bonusStatistics)
        {
            // statistics increases in non-dumb way - we don't want to increase base statistics, only bonus ones
            bonusAttack += bonusStatistics.bonusAttack;
            bonusDefence += bonusStatistics.bonusDefence;
            bonusAttackRange += bonusStatistics.bonusAttackRange;
            maxHealthPoints += bonusStatistics.bonusHealth;
            bonusMaxMovementPoints += bonusStatistics.bonusMovementPoints;
            numberOfAttacks += bonusStatistics.bonusNumberOfAttacks;
            currentMaxNumberOfRetaliations += bonusStatistics.bonusNumberOfRetaliations;
            numberOfRetaliations += bonusStatistics.bonusNumberOfRetaliations;

            healthPoints += bonusStatistics.bonusHealth;
            movementPoints += bonusStatistics.bonusMovementPoints;
        }

        public void RemoveBonusStatistics(ChangeableStatistics bonusStatistics)
        {
            bonusAttack -= bonusStatistics.bonusAttack;
            bonusDefence -= bonusStatistics.bonusDefence;
            bonusAttackRange -= bonusStatistics.bonusAttackRange;
            maxHealthPoints -= bonusStatistics.bonusHealth;

            if(healthPoints > maxHealthPoints)
            {
                healthPoints = maxHealthPoints;
            }
        }
    }
}
