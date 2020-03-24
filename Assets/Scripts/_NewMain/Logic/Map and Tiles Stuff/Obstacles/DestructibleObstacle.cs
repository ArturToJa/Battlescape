using UnityEngine;
using System.Collections.Generic;

namespace BattlescapeLogic
{
    public class DestructibleObstacle : Obstacle, IDamageable
    {
        [SerializeField] string obstacleName;

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

        BattlescapeUI.AbstractHealthbar myHealthBar;



        public BuffGroup buffs { get; private set; }




        public override void Start()
        {
            base.Start();
            buffs = new BuffGroup(this);
            myHealthBar = GetComponentInChildren<BattlescapeUI.DestructibleObstacleHealthbar>();
            currentHealthPoints = maxHealthPoints;
        }

        public void TakeDamage(Unit source, int dmg)
        {
            currentHealthPoints -= dmg;

            if (currentHealthPoints <= 0)
            {
                this.Destruct(source);
            }
        }

        public override void OnMouseHoverEnter()
        {
            myHealthBar.TurnOn();
        }

        public override void OnMouseHoverExit()
        {
            myHealthBar.TurnOff();
            UIHitChanceInformation.instance.TurnOff();
        }

        public Player GetMyOwner()
        {
            return null;
        }

        public Vector3 GetMyPosition()
        {
            return transform.position;
        }

        public int GetCurrentDefence()
        {
            return 0;
        }

        public string GetMyName()
        {
            return obstacleName;
        }

        public float ChanceOfBeingHitBy(Unit source)
        {
            return 1;
        }
    }
}
