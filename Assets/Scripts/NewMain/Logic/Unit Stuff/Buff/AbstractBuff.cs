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
                if(IsExpired())
                {
                    OnExpire();
                }
            }
        }

        public Unit owner { get; private set; }
        public AbstractAbility source { get; private set; }

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
            return;
        }
        public override void OnNewPhase()
        {
            return;
        }

        public bool IsExpired()
        {
            return !HasInfiniteDuration() && !(duration < 0);
        }

        public bool HasInfiniteDuration()
        {
            return duration < 0;
        }

        protected virtual void OnExpire()
        {
            OnDestruction();
            RemoveChange();
        }

        public abstract void ApplyChange();
        protected abstract void RemoveChange();
    }
}
