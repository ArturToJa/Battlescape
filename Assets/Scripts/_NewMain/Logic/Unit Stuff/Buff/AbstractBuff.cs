using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattlescapeLogic
{
    public abstract class AbstractBuff : Expirable
    {
        [SerializeField] bool _isHidden = false;
        public bool isHidden
        {
            get
            {
                return _isHidden;
            }
        }
        [SerializeField] string _buffName;
        public string buffName
        {
            get
            {
                return _buffName;
            }
            private set
            {
                _buffName = value;
            }
        }

        [SerializeField] string _description;
        public string description
        {
            get
            {
                return _description;
            }
            private set
            {
                _description = value;
            }
        }

        [SerializeField] Sprite _image;
        public Sprite icon
        {
            get
            {
                return _image;
            }
            private set
            {
                _image = value;
            }
        }

        [SerializeField] bool _isStackable;
        public bool isStackable
        {
            get
            {
                return _isStackable;
            }
            private set
            {
                _isStackable = value;
            }
        }

        public BuffGroup buffGroup { get; private set; }
        public AbstractAbility source { get; private set; }

        public int index { get; private set; }


        public static event System.Action<AbstractBuff> OnBuffCreation;
        public static event System.Action<AbstractBuff> OnBuffDestruction;

        [SerializeField] GameObject visualEffectPrefab;
        GameObject visualEffect;

        public override void OnNewTurn()
        {
            base.OnNewTurn();
            if (IsExpired() && GameRound.instance.currentPlayer == source.owner.GetMyOwner())
            {
                OnExpire();
            }
        }

        protected override void OnExpire()
        {
            if (visualEffect != null)
            {
                Destroy(visualEffect);
            }
            RemoveChange();
            base.OnExpire();
            this.buffGroup.RemoveBuff(this);
            OnBuffDestruction(this);
        }

        protected abstract bool IsAcceptableTargetType(IDamageable target);

        public void ApplyOnTarget(IDamageable target, AbstractAbility source)
        {
            if(IsAcceptableTargetType(target))
            {
                this.source = source;
                ApplyOnTarget(target);
            }
            else
            {
                OnDestruction();
            }
        }

        public void ApplyOnTarget(IDamageable target)
        {
            if(IsAcceptableTargetType(target))
            {
                if (visualEffectPrefab != null)
                {
                    visualEffect = Instantiate(visualEffectPrefab, transform.position, visualEffectPrefab.transform.rotation);
                }
                if (!isStackable && IsAlreadyOnTarget(target))
                {
                    OnDestruction();
                }
                else
                {
                    buffGroup = target.buffs;
                    target.buffs.AddBuff(this);
                    ApplyChange();
                }
                OnBuffCreation(this);
            }
            else
            {
                OnDestruction();
            }
        }

        public void RemoveFromTargetInstantly()
        {
            OnExpire();
        }

        protected bool IsAlreadyOnTarget(IDamageable target)
        {
            return !target.buffs.FindAllBuffsOfType(this.name).IsEmpty();
        }
        public abstract void ApplyChange();
        protected abstract void RemoveChange();
    }
}
