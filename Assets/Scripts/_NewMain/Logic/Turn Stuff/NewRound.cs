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
    public abstract class TurnChangeObject : ITurnInteractable
    {
        public TurnChangeObject()
        {
            OnCreation();
        }
        public abstract void OnNewRound();
        public abstract void OnNewTurn();
        public abstract void OnNewPhase();

        public virtual void OnCreation()
        {
            GameRound.instance.newRoundObjects.AddLast(this);
        }

        public virtual void OnDestruction()
        {
            GameRound.instance.newRoundObjects.Remove(GameRound.instance.newRoundObjects.Find(this));
        }
    }

    public abstract class TurnChangeMonoBehaviour : MonoBehaviour, ITurnInteractable
    {
        public abstract void OnNewRound();
        public abstract void OnNewTurn();
        public abstract void OnNewPhase();

        public virtual void OnCreation()
        {
            GameRound.instance.newRoundObjects.AddLast(this);
        }

        public virtual void OnDestruction()
        {
            GameRound.instance.newRoundObjects.Remove(GameRound.instance.newRoundObjects.Find(this));
        }

        protected virtual void Start()
        {
            OnCreation();
        }
    }
}
