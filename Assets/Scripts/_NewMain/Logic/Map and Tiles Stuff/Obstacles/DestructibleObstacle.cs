using UnityEngine;
using System.Collections.Generic;

namespace BattlescapeLogic
{
    public class DestructibleObstacle : Obstacle, IDamageable
    {
        public BuffGroup buffs { get; private set; }

        [SerializeField] int _healthpoints;
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

        public override void Start()
        {
            base.Start();
            buffs = new BuffGroup(this);
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
