using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattlescapeLogic
{
    [System.Serializable]
	public class ChangeableStatistics
	{
        [SerializeField] int _bonusAttack;
        public int bonusAttack
        {
            get
            {
                return _bonusAttack;
            }
            private set
            {
                _bonusAttack = value;
            }
        }

        [SerializeField] int _bonusDefence;
        public int bonusDefence
        {
            get
            {
                return _bonusDefence;
            }
            private set
            {
                _bonusDefence = value;
            }
        }

        [SerializeField] int _bonusHealth;
        public int bonusHealth
        {
            get
            {
                return _bonusHealth;
            }
            private set
            {
                _bonusHealth = value;
            }
        }

        [SerializeField] int _bonusMovementPoints;
        public int bonusMovementPoints
        {
            get
            {
                return _bonusMovementPoints;
            }
            private set
            {
                _bonusMovementPoints = value;
            }
        }

        [SerializeField] int _bonusAttackRange;
        public int bonusAttackRange
        {
            get
            {
                return _bonusAttackRange;
            }
            private set
            {
                _bonusAttackRange = value;
            }
        }

        [SerializeField] int _bonusNumberOfAttacks;
        public int bonusNumberOfAttacks
        {
            get
            {
                return _bonusNumberOfAttacks;
            }
            private set
            {
                _bonusNumberOfAttacks = value;
            }
        }

        [SerializeField] int _bonusNumberOfRetaliations;
        public int bonusNumberOfRetaliations
        {
            get
            {
                return _bonusNumberOfRetaliations;
            }
            private set
            {
                _bonusNumberOfRetaliations = value;
            }
        }

        [SerializeField] int _bonusEnergyRegen;
        public int bonusEnergyRegen
        {
            get
            {
                return _bonusEnergyRegen;
            }
            private set
            {
                _bonusEnergyRegen = value;
            }
        }
    }
}

