using UnityEngine;

namespace BattlescapeLogic
{

    public interface INewRound
    {
        void OnNewRound();
        void OnCreation();
        void OnDestruction();
    }
    public abstract class NewRound : INewRound
    {
        public NewRound()
        {
            OnCreation();
        }
        public abstract void OnNewRound();

        public virtual void OnCreation()
        {
            GameRound.instance.newRoundObjects.AddLast(this);
        }

        public virtual void OnDestruction()
        {
            GameRound.instance.newRoundObjects.Remove(GameRound.instance.newRoundObjects.Find(this));
        }
    }

    public abstract class NewRoundMonoBehaviour : MonoBehaviour, INewRound
    {
        public abstract void OnNewRound();

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
