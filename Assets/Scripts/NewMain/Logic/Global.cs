using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattlescapeLogic
{
    public class Global : MonoBehaviour
    {
        public static Global instance { get; private set; }
        public NewMap map { get; private set; }
        public List<PlayerTeam> playerTeams { get; private set; }

        //THIS IS TEMPORARY!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        public PlayerBuilder[] playerBuilders = new PlayerBuilder[2];

        public AbstractMovement[] movementTypes = new AbstractMovement[3];

        void Start()
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
        }


        public Player GetNextPlayer(Player player)
        {
            if (playerTeams.Count > player.team.index && playerTeams[player.team.index] != null && playerTeams[player.team.index].Players.Count > player.index && playerTeams[player.team.index].Players[player.index] != null)
            {
                if (playerTeams[player.team.index].Players.Count - 1 > player.index)
                //meaning 'there are NEXT players in this team - I assume currently players move first inside of one team, might be false!
                {
                    return player.team.Players[player.index + 1];
                }
                else if (playerTeams.Count - 1 > player.team.index)
                //meaning 'there is a NEXT team - same assumption as before
                {
                    return playerTeams[player.team.index + 1].Players[0];
                }
                else
                {
                    //there are NO 'next' teams and players, so its time for player 0 of team 0!
                    return playerTeams[0].Players[0];
                }
            }
            else
            {
                //havent found the Player player in the lists - freak out with an error!
                Debug.LogError("Fuck, no such player exists!");
                return null;
            }                        
        }
    }
}
