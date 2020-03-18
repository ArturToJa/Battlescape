using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattlescapeLogic
{
    public abstract class AbstractBuff : TurnChangeMonoBehaviour
    {
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

        [SerializeField] int _duration;
        public int duration
        {
            get
            {
                return _duration;
            }
            private set
            {
                _duration = value;
                //if(IsExpired())
                //{
                //    OnExpire();
                //}
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

        public Unit owner { get; private set; }
        public AbstractAbility source { get; private set; }

        public int index { get; private set; }


        public static event System.Action<AbstractBuff> OnBuffCreation;
        public static event System.Action<AbstractBuff> OnBuffDestruction;

        [SerializeField] GameObject visualEffectPrefab;
        GameObject visualEffect;

        protected override void Start()
        {
            base.Start();
        }

        public override void OnNewRound()
        {
            if(!IsExpired())
            {
                duration--;
            }
        }

        public override void OnNewTurn()
        {
            if (IsExpired() && GameRound.instance.currentPlayer == owner.owner)
            {
                OnExpire();
            }
        }
        public override void OnNewPhase()
        {
            return;
        }

        public bool IsExpired()
        {
            return !HasInfiniteDuration() && (duration == 0);
        }

        public bool HasInfiniteDuration()
        {
            return duration < 0;
        }

        protected virtual void OnExpire()
        {
            if (visualEffect != null)
            {
                Destroy(visualEffect);
            }
            OnDestruction();
            this.owner.buffs.Remove(this);
            RemoveChange();
            OnBuffDestruction(this);
        }

        public void ApplyOnUnit(Unit unit, AbstractAbility source)
        {
            this.source = source;
            ApplyOnUnit(unit);
        }

        public void ApplyOnUnit(Unit unit)
        {
            if (visualEffectPrefab != null)
            {
                visualEffect = Instantiate(visualEffectPrefab, unit.transform.position, visualEffectPrefab.transform.rotation);
            }
            if(!isStackable && IsAlreadyOnUnit(unit))
            {
                OnDestruction();
            }
            else
            {
                this.owner = unit;
                unit.buffs.Add(this);
                ApplyChange();
            }
            OnBuffCreation(this);
        }

        public void RemoveFromUnitInstantly()
        {
            OnExpire();
        }

        protected bool IsAlreadyOnUnit(Unit unit)
        {
            return unit.FindAllBuffsOfType(this.name).Count > 0;
        }
        public abstract void ApplyChange();
        protected abstract void RemoveChange();
    }
}
