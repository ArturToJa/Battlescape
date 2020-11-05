using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace BattlescapeLogic
{

    public interface ITurnInteractable
    {
        void OnNewRound();
        void OnNewTurn();
        void OnNewPhase();
        void OnCreation();
        void OnDestruction();
    }
    public class TurnChanger : ITurnInteractable
    {
        private Action OnNewRound_ = null;
        private Action OnNewTurn_ = null;
        private Action OnNewPhase_ = null;
        private Action OnNewPlayerRound_ = null;
        private Player owner = null;

        public TurnChanger(Action onNewRound, Action onNewTurn, Action onNewPhase)
        {
            OnNewRound_ = onNewRound;
            OnNewTurn_ = onNewTurn;
            OnNewPhase_ = onNewPhase;
            OnCreation();
        }
        public TurnChanger(Player owner, Action onNewRound, Action onNewTurn, Action onNewPhase, Action onNewPlayerRound)
        {
            OnNewRound_ = onNewRound;
            OnNewTurn_ = onNewTurn;
            OnNewPhase_ = onNewPhase;
            OnNewPlayerRound_ = onNewPlayerRound;
            this.owner = owner;
            OnCreation();
        }
        public void OnNewRound()
        {
            if(OnNewRound_ != null)
            {
                OnNewRound_();
            }
            else
            {
                Debug.Log("No OnNewRound method implemented!");
            }
            
        }
        public void OnNewTurn()
        {
            if (OnNewTurn_ != null)
            {
                OnNewTurn_();
            }
            else
            {
                Debug.Log("No OnNewTurn method implemented!");
            }
        }
        public void OnNewPhase()
        {
            if (OnNewPhase_ != null)
            {
                OnNewPhase_();
            }
            else
            {
                Debug.Log("No OnNewPhase method implemented!");
            }
        }

        public void OnNewPlayerRound()
        {
            if (OnNewPlayerRound_ != null)
            {
                OnNewPlayerRound_();
            }
            else
            {
                // This is fine, not all object have PlayerRound
            }
        }

        public void OnCreation()
        {
            if(owner != null)
            {
                GameRound.instance.turnChangerObjects[owner].AddLast(this);
            }
            GameRound.instance.newRoundObjects.AddLast(this);
        }

        public void OnDestruction()
        {
            if(owner != null)
            {
                GameRound.instance.turnChangerObjects[owner].Remove(GameRound.instance.turnChangerObjects[owner].Find(this));
            }
            GameRound.instance.newRoundObjects.Remove(GameRound.instance.newRoundObjects.Find(this));
        }
    }
}
