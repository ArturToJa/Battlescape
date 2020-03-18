using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattlescapeLogic
{
    public class DestructibleObstacle : Obstacle
    {
        [SerializeField] int _maxHealth;
        public int maxHealth
        {
            get
            {
                return _maxHealth;
            }
            private set
            {
                _maxHealth = value;
            }
        }
        public int currentHealth { get; private set; }
    }
}