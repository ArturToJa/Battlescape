using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattlescapeLogic
{
    [Serializable]
    public class Expirable
{
        TurnChanger turnChanger;
        Action OnExpire_ = null;
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

        public Expirable(Player owner, Action onExpire)
        {
            turnChanger = new TurnChanger(owner, OnNewRound, OnNewTurn, OnNewPhase, OnNewPlayerRound);
            OnExpire_ = onExpire;
        }

        public bool IsExpired()
        {
            return !HasInfiniteDuration() && (duration == 0);
        }

        public bool HasInfiniteDuration()
        {
            return duration < 0;
        }

        private void OnExpire()
        {
            if(OnExpire_ != null)
            {
                OnExpire_();
            }
            turnChanger.OnDestruction();
        }

        public void ExpireNow()
        {
            OnExpire();
        }

        public void OnNewRound()
        {
        }

        public void OnNewTurn()
        {
        }

        public void OnNewPhase()
        {
        }

        public void OnNewPlayerRound()
        {
            if (!IsExpired())
            {
                duration--;
            }
        }
    }
}
