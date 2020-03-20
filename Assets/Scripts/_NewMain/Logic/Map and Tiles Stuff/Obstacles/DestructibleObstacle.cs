using UnityEngine;
using System.Collections.Generic;

namespace BattlescapeLogic
{
    public class DestructibleObstacle : Obstacle, IDamageable
    {
        [SerializeField]
        int _healthpoints;

        public List<AbstractBuff> currentBuffs = new List<AbstractBuff>();

        public int healthPoints
        {
            get
            {
                return _healthpoints;
            }

            private set
            {
                _healthpoints = value;
            }
        }

        public void TakeDamage(Unit source, int dmg)
        {
            healthPoints -= dmg;

            if(healthPoints<=0)
            {
                this.Destruct(source);
            }
        }
    }
}
