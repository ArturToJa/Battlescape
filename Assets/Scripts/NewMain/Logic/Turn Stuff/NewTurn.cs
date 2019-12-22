using UnityEngine;

namespace BattlescapeLogic
{

    public interface INewTurn
    {
        void OnNewTurn();
        void OnCreation();
        void OnDestruction();
    }
    public abstract class NewTurn : INewTurn
    {
        public NewTurn()
        {
            OnCreation();
        }
        public abstract void OnNewTurn();

        public virtual void OnCreation()
        {
            GameTurn.instance.newTurnObjects.AddLast(this);
        }

        public virtual void OnDestruction()
        {
            GameTurn.instance.newTurnObjects.Remove(GameTurn.instance.newTurnObjects.Find(this));
        }
    }

    public abstract class NewTurnMonoBehaviour : MonoBehaviour, INewTurn
    {
        public abstract void OnNewTurn();

        public virtual void OnCreation()
        {
            GameTurn.instance.newTurnObjects.AddLast(this);
        }

        public virtual void OnDestruction()
        {
            GameTurn.instance.newTurnObjects.Remove(GameTurn.instance.newTurnObjects.Find(this));
        }

        protected virtual void Start()
        {
            OnCreation();
        }
    }
}
