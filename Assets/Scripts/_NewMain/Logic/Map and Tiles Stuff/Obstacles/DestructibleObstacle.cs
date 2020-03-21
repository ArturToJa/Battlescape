using UnityEngine;
using System.Collections.Generic;

namespace BattlescapeLogic
{
    public class DestructibleObstacle : Obstacle, IDamageable
    {
        public BuffGroup buffs { get; private set; }

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

        public int currentHealthPoints { get; private set; }


        public override void Start()
        {
            base.Start();
            buffs = new BuffGroup(this);
        }

        public void TakeDamage(Unit source, int dmg)
        {
            currentHealthPoints -= dmg;

            if (currentHealthPoints <= 0)
            {
                this.Destruct(source);
            }
        }
    }
}
