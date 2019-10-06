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
