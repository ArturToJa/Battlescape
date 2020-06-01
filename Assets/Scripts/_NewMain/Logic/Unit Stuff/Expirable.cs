using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattlescapeLogic
{
    public class Expirable : TurnChangeMonoBehaviour
{
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
            }
        }

        protected override void Start()
        {
            base.Start();
        }

        public virtual bool IsExpired()
        {
            return !HasInfiniteDuration() && (duration == 0);
        }

        public virtual bool HasInfiniteDuration()
        {
            return duration < 0;
        }

        protected virtual void OnExpire()
        {
            OnDestruction();
        }

        public override void OnNewRound()
        {
            if (!IsExpired())
            {
                duration--;
            }
        }

        public override void OnNewTurn()
        {
        }

        public override void OnNewPhase()
        {
        }
    }
}
