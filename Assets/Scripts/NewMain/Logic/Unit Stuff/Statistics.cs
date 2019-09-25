using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattlescapeLogic
{
    public class Statistics
    {
        public float baseAttack { get; private set; }
        public float bonusAttack { get; set; }
        public float baseDefence { get; private set; }
        public float bonusDefence { get; set; }
        public float maxHealthPoints { get; private set; }
        public float healthPoints { get; set; }
        public int maxMovementPoints { get; private set; }
        public int movementPoints { get; set; }

        public Statistics(float baseAttack, float baseDefence, float maxHealthPoints, int maxMovementPoints)
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
