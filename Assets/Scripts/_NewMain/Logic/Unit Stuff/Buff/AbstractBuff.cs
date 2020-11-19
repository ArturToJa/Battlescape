using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattlescapeLogic
{
    public abstract class AbstractBuff : MonoBehaviour
    {
        [SerializeField] int duration;
        public Expirable expirable { get; private set; }


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

        protected void OnExpire()
        {
            Debug.Log("Buff expired");
            if (visualEffect != null)
            {
                Destroy(visualEffect);
            }
            RemoveChange();
            this.buffGroup.RemoveBuff(this);
            OnBuffDestruction(this);
        }

        protected abstract bool IsAcceptableTargetType(IDamageable target);

        public void ApplyOnTarget(IDamageable target, AbstractAbility source)
        {
            if(IsAcceptableTargetType(target))
            {
                expirable = new Expirable(target.GetMyOwner(), OnExpire, duration);
                this.source = source;
                ApplyOnTarget(target);
            }
        }

        public void ApplyOnTarget(IDamageable target)
        {
            if(IsAcceptableTargetType(target))
            {
                if(isStackable || !IsAlreadyOnTarget(target))
                {
                    if (visualEffectPrefab != null)
                    {
                        visualEffect = Instantiate(visualEffectPrefab, transform.position, visualEffectPrefab.transform.rotation);
                    }
                    buffGroup = target.buffs;
                    target.buffs.AddBuff(this);
                    ApplyChange();
                }
                OnBuffCreation(this);
            }
        }

        public void RemoveFromTargetInstantly()
        {
            expirable.ExpireNow();
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
