using UnityEngine;

namespace BattlescapeLogic
{
    public class FakeDamageTarget : IDamageable
    {
        int defence;
        public BuffGroup buffs => new BuffGroup();

        public MultiTile currentPosition => null;

        public FakeDamageTarget(int _defence)
        {
            defence = _defence;
        }

        public float ChanceOfBeingHitBy(IDamageSource source)
        {
            return Maths.Sigmoid(DamageCalculator.GetStatisticsDifference(source, this), DamageCalculator.sigmoidGrowthRate);
        }

        public int GetCurrentDefence()
        {
            return defence;
        }

        public string GetMyName()
        {
            throw new System.NotImplementedException();
        }

        public Player GetMyOwner()
        {
            throw new System.NotImplementedException();
        }

        public Vector3 GetMyPosition()
        {
            throw new System.NotImplementedException();
        }

        public bool IsInvulnerable()
        {
            return false;
        }

        public void OnHitReceived(Damage damage)
        {
            throw new System.NotImplementedException();
        }

        public void OnHitReceivedWhenInvulnerable(Damage damage)
        {
            throw new System.NotImplementedException();
        }

        public void OnMissReceived(Damage damage)
        {
            throw new System.NotImplementedException();
        }

        public void OnMouseHoverEnter(Vector3 exactMousePosition)
        {
            throw new System.NotImplementedException();
        }

        public void OnMouseHoverExit()
        {
            throw new System.NotImplementedException();
        }

        public void TakeDamage(Damage dmg)
        {
            throw new System.NotImplementedException();
        }
    }
}