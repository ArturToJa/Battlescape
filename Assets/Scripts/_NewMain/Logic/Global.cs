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
        public MatchTypes matchType;

        //THIS IS TEMPORARY!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        public PlayerBuilder[,] playerBuilders = new PlayerBuilder[2,1];

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
            playerBuilders[0,0] = new PlayerBuilder();
            playerBuilders[1,0] = new PlayerBuilder();
            matchType = MatchTypes.None;
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
            if (playerBuilders[GameRound.instance.currentPlayer.team.index,GameRound.instance.currentPlayer.index] != null)
            {
                return playerBuilders[GameRound.instance.currentPlayer.team.index, GameRound.instance.currentPlayer.index].type == PlayerType.Local;
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
            foreach (PlayerBuilder playerBuilder in playerBuilders)
            {
                if (playerBuilder != null && playerBuilder.isObserver == false)
                {
                    count++;
                }
            }
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

        public static List<IMouseTargetable> FindAllTargetablesInLine(Vector3 start, Vector3 end)
        {
            var VectorToTarget = -start + end;

            Ray ray = new Ray(start, VectorToTarget);
            RaycastHit[] hits = Physics.RaycastAll(ray, Vector3.Distance(start, end));

            var list = new List<IMouseTargetable>();

            foreach (var hit in hits)
            {
                if (hit.transform.GetComponent<IMouseTargetable>() != null)
                {
                    list.Add(hit.transform.GetComponent<IMouseTargetable>());
                }
            }
            return list;
        }
    }
}
