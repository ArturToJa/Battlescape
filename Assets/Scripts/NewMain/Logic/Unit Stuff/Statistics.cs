using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattlescapeLogic
{
    [System.Serializable]
    public class Statistics
    {
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
        public float bonusAttack { get; set; }
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
        public float bonusDefence { get; set; }
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
        public float healthPoints { get; set; }
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

        public Statistics(int baseAttack, int baseDefence, int maxHealthPoints, int maxMovementPoints)
        {
            this.baseAttack = baseAttack;
            this.baseDefence = baseDefence;
            this.maxHealthPoints = maxHealthPoints;
            this.maxMovementPoints = maxMovementPoints;

            this.bonusAttack = 0.0f;
            this.bonusDefence = 0.0f;
            this.healthPoints = this.maxHealthPoints;
            this.movementPoints = this.maxMovementPoints;
        }

        public Statistics(Statistics other)
        {
            this.baseAttack = other.baseAttack;
            this.baseDefence = other.baseDefence;
            this.maxHealthPoints = other.maxHealthPoints;
            this.maxMovementPoints = other.maxMovementPoints;

            this.bonusAttack = other.bonusAttack;
            this.bonusDefence = other.bonusDefence;
            this.healthPoints = other.healthPoints;
            this.movementPoints = other.movementPoints;
        }
    }
}
