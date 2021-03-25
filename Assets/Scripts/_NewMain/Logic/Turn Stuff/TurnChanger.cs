using System;
using UnityEngine;

namespace BattlescapeLogic
{
    public class TurnChanger : ITurnInteractable
    {
        private Action OnNewRound_ = null;
        private Action OnNewTurn_ = null;
        private Action OnNewPhase_ = null;
        private Action OnOwnerTurn_ = null;
        private Player owner = null;

        public TurnChanger(Action onNewRound, Action onNewTurn, Action onNewPhase)
        {
            OnNewRound_ = onNewRound;
            OnNewTurn_ = onNewTurn;
            OnNewPhase_ = onNewPhase;
            OnCreation();
        }
        public TurnChanger(Player owner, Action onNewRound, Action onNewTurn, Action onNewPhase, Action onOwnerTurn)
        {
            OnNewRound_ = onNewRound;
            OnNewTurn_ = onNewTurn;
            OnNewPhase_ = onNewPhase;
            OnOwnerTurn_ = onOwnerTurn;
            this.owner = owner;
            if (owner == null)
            {
                Debug.LogWarning("Owner is null!");
            }
            OnCreation();
        }
        public void OnNewRound()
        {
            if (OnNewRound_ != null)
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

        public void OnOwnerTurn()
        {
            if (OnOwnerTurn_ != null)
            {
                //Debug.Log("Woof");
                OnOwnerTurn_();
            }
            else
            {
                // This is fine, not all object have PlayerRound
            }
        }

        public void OnCreation()
        {
            if (owner != null)
            {
                GameRound.instance.objectsToUpdateOnOwnerTurn[owner].AddLast(this);
            }

            GameRound.instance.objectsToUpdateOnRoundsTurnsAndPhases.AddLast(this);
        }

        public void OnDestruction()
        {
            if (owner != null)
            {
                GameRound.instance.objectsToUpdateOnOwnerTurn[owner].Remove(GameRound.instance.objectsToUpdateOnOwnerTurn[owner].Find(this));
            }
            else
            {
                GameRound.instance.objectsToUpdateOnRoundsTurnsAndPhases.Remove(GameRound.instance.objectsToUpdateOnRoundsTurnsAndPhases.Find(this));
            }
        }
    }
}
