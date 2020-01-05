using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattlescapeLogic
{

    public enum MatchTypes { Online, HotSeat, Singleplayer, None }

    public class Global : MonoBehaviour
    {
        public static Global instance { get; private set; }
        public NewMap map { get; private set; }
        public List<PlayerTeam> playerTeams { get; private set; }
        public MatchTypes MatchType;

        //THIS IS TEMPORARY!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        public PlayerBuilder[] playerBuilders = new PlayerBuilder[2];

        public AbstractMovement[] movementTypes = new AbstractMovement[3];       

        void Awake()
        {
            DontDestroyOnLoad(this.gameObject);
            if (instance == null)
            {
                instance = this;
            }
            else if (instance != this)
            {                
                Destroy(this.gameObject);
            }
            movementTypes[(int)MovementTypes.Ground] = new GroundMovement();
            movementTypes[(int)MovementTypes.Flying] = new FlyingMovement();
            movementTypes[(int)MovementTypes.None] = null;
            playerTeams = new List<PlayerTeam>();
            PlayerTeam teamLeft = new PlayerTeam(0, 1);
            PlayerTeam teamRight = new PlayerTeam(1, 1);
            playerTeams.Add(teamLeft);
            playerTeams.Add(teamRight);
            playerBuilders[0] = new PlayerBuilder();
            playerBuilders[1] = new PlayerBuilder();
            MatchType = MatchTypes.None;
        }


        public Player GetNextPlayer(Player player)
        {
            if (playerTeams.Count > player.team.index && 
                playerTeams[player.team.index] != null && 
                playerTeams[player.team.index].players.Count > player.index && 
                playerTeams[player.team.index].players[player.index] != null)
            {
                if (playerTeams[player.team.index].players.Count - 1 > player.index)
                //meaning 'there are NEXT players in this team - I assume currently players move first inside of one team, might be false!
                {
                    return player.team.players[player.index + 1];
                }
                else if (playerTeams.Count - 1 > player.team.index)
                //meaning 'there is a NEXT team - same assumption as before
                {
                    return playerTeams[player.team.index + 1].players[0];
                }
                else
                {
                    //there are NO 'next' teams and players, so its time for player 0 of team 0!
                    return playerTeams[0].players[0];
                }
            }
            else
            {
                //havent found the Player player in the lists - freak out with an error!
                Debug.LogError("Fuck, no such player exists!");
                return null;
            }                        
        }

        public bool IsCurrentPlayerLocal()
        {
            if (playerBuilders[GameRound.instance.currentPlayer.team.index] != null)
            {
                return playerBuilders[GameRound.instance.currentPlayer.team.index].type == PlayerType.Local;
            }
            else
            {
                return (GameRound.instance.currentPlayer.type == PlayerType.Local);
            }
        }

        public List<Unit> GetAllUnits()
        {
            List<Unit> returnList = new List<Unit>();
            foreach (PlayerTeam team in playerTeams)
            {
                foreach (Player player in team.players)
                {
                    foreach (Unit unit in player.playerUnits)
                    {
                        returnList.Add(unit);
                    }
                }
            }
            return returnList;
        }

        //Doesn't count the observers
        public int GetActivePlayerCount()
        {
            int count = 0;
            if (playerBuilders[0] != null)
            {
                foreach (PlayerBuilder player in playerBuilders)
                {
                    if (player.isObserver == false)
                    {
                        count++;
                    }
                }
            }
            else
            {
                foreach (PlayerTeam team in playerTeams)
                {
                    foreach (Player player in team.players)
                    {
                        if (player.isObserver == false)
                        {
                            count++;
                        }
                    }
                }
            }
            return count;
        }


    }
}
