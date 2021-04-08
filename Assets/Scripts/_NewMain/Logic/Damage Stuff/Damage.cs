using UnityEngine;
using System;

namespace BattlescapeLogic
{
    public class Damage
    {
        public Damage(int _baseDamage, bool _isHit, IDamageSource _source)
        {
            baseDamage = _baseDamage;
            isHit = _isHit;
            source = _source;
        }

        public int baseDamage { get; private set; }
        public bool isHit { get; private set; }
        public int bonusDamage = 0;
        public int bonusPercentDamage = 0;

        public IDamageSource source;

        public static event Action OnInvulnerableTargetHit = delegate { };
        public static event Action<Damage, IDamageable> OnDamageDealt = delegate { };
        //NOTE: this may be a miss too!!!!

        public int GetTotalDamage()
        {
            return baseDamage + bonusDamage + baseDamage * bonusPercentDamage / 100;
        }

        public static implicit operator int(Damage damage)
        {
            return damage.isHit ? damage.GetTotalDamage() : 0;
        }

        public static implicit operator string(Damage damage)
        {
            return (damage.isHit ? damage.GetTotalDamage() : 0).ToString();
        }


        public void Deal(IDamageable target)
        {
            PlayerInput.instance.UnlockInput();
            ApplyDamageModifiers(target);

            if (target.IsInvulnerable())
            {
                target.OnHitReceivedWhenInvulnerable(this);
            }

            else if (isHit)
            {
                target.OnHitReceived(this);
            }
            else
            {
                target.OnMissReceived(this);                
            }
            OnDamageDealt(this, target);
        }

        void ApplyDamageModifiers(IDamageable target)
        {           
            foreach (AbstractAttackModifier modifier in source.GetMyOwner().modifiers)
            {
                modifier.ModifyDamage(this);
            }
            foreach (AbstractAttackModifier modifier in source.GetMyOwner().modifiers)
            {
                modifier.ModifyAttack(target, this);
            }
        }
    }
}