using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattlescapeLogic
{
    [System.Serializable]
    public class Statistics
    {
        public static readonly int baseDamage = 10;

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
        [SerializeField] int __baseDefence;
        public int baseDefence
        {
            get
            {
                return __baseDefence;
            }
            private set
            {
                __baseDefence = value;
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
        [SerializeField] int _maxMovementPoints;
        public int maxMovementPoints
        {
            get
            {
                return _maxMovementPoints;
            }
            private set
            {
                _maxMovementPoints = value;
            }
        }
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

        public void NullMaxMovementPoints()
        {
            _maxMovementPoints = 0;
        }

        public void NullBaseAttack()
        {
            _baseAttack = 0;
        }
    }
}
