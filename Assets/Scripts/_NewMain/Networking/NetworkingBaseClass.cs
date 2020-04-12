using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BattlescapeLogic
{
    //Maybe this class requires SOME refactors :D it's mostly copy-pasted from old classes being deleted. COntains ALL RPCs and their Commands.
    //Also - if this class stays as is, it is NOT used only for networked games, but for ALL

    public abstract class NetworkingBaseClass : MonoBehaviour
    {
        #region static
        public static NetworkingBaseClass Instance { get; set; }
        #endregion

        #region Unity Methods
        protected virtual void Start()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            DontDestroyOnLoad(gameObject);
        }
        #endregion

        #region abstract
        public abstract void Connect();
        public abstract void JoinRoom(string roomName);

        public abstract void Disconnect();

        public abstract bool IsConnected();

        public abstract void SendCommandToAddPlayer(PlayerTeam playerTeam, Player player);

        public abstract void SendCommandToAddObstacles();

        public abstract void SendCommandToDestroyObstacle(Unit sourceUnit, Obstacle myObstacle);

        public abstract void SendCommandToMove(Unit unit, Tile destination);

        public abstract void SendCommandToStartAttack(Unit attackingUnit, IDamageable target);

        public abstract void SendCommandToGiveChoiceOfRetaliation(Unit retaliatingUnit, Unit target);

        public abstract void SendCommandToRetaliate();

        public abstract void SendCommandToHit(Unit source, IDamageable target, int damage = -1);

        public abstract void SendCommandToNotRetaliate();

        public abstract void SendCommandToEndTurnPhase();

        public abstract void SetHeroName(int playerTeamIndex, string heroName);

        public abstract void SendInfoToOthersThatDisconnected();

        public abstract void PlayerEndedPreGame();
        
        #endregion
    }
}
