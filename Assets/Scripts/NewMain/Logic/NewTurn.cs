using UnityEngine;

namespace BattlescapeLogic
{

    public interface INewTurn
    {
        void OnNewTurn();
        void OnCreation();
        void OnDestruction();
    }
    public class NewTurn : INewTurn
    {
        public NewTurn()
        {
            OnCreation();
        }
        public virtual void OnNewTurn()
        {

        }

        public virtual void OnCreation()
        {
            GameTurn.instance.newTurnObjects.AddLast(this);
        }

        public virtual void OnDestruction()
        {
            GameTurn.instance.newTurnObjects.Remove(GameTurn.instance.newTurnObjects.Find(this));
        }
    }

    public class NewTurnMonoBehaviour : MonoBehaviour, INewTurn
    {
        public virtual void OnNewTurn()
        {

        }

        public virtual void OnCreation()
        {
            GameTurn.instance.newTurnObjects.AddLast(this);
        }

        public virtual void OnDestruction()
        {
            GameTurn.instance.newTurnObjects.Remove(GameTurn.instance.newTurnObjects.Find(this));
        }

        public virtual void Start()
        {
            OnCreation();
        }
    }
}
