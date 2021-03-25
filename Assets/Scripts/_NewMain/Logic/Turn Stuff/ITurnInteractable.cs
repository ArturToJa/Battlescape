using System.Runtime.CompilerServices;

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
}
