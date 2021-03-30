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




        protected new void Start()
        {
            base.Start();
            buffs = new BuffGroup(this);
            myHealthBar = GetComponentInChildren<BattlescapeUI.DestructibleObstacleHealthbar>();
            currentHealthPoints = maxHealthPoints;
        }

        public void TakeDamage(Damage damage)
        {
            currentHealthPoints -= damage;

            if (currentHealthPoints <= 0)
            {
                this.Destruct(damage.source);
            }
        }

        public override void OnMouseHoverEnter(Vector3 exactMousePosition)
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

        public float ChanceOfBeingHitBy(IDamageSource source)
        {
            return 1;
        }

        public bool IsInvulnerable()
        {
            return false;
        }

        public void OnHitReceived(Damage damage)
        {
            StartCoroutine((myHealthBar as BattlescapeUI.DestructibleObstacleHealthbar).ShowOnDamageRoutine());
            TakeDamage(damage);
        }

        public void OnMissReceived(Damage damage)
        {
            Debug.LogWarning("If missing on obstacles is now a thing, remove this warning; otherwise this is a bug.");
        }

        public void OnHitReceivedWhenInvulnerable(Damage damage)
        {
            Debug.LogWarning("If invulnerable obstacles are a thing now, please remove this warning. Else, this is a bug!");
        }
    }
}
