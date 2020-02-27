using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattlescapeLogic
{
    public class PlayerTeam
    {
        public PlayerTeam(int index, int numberOfPlayers)
        {
            this.index = index;
            players = new List<Player>(numberOfPlayers);
        }

        public int index;
        public List<Player> players;

        Player GetPlayerByIndex(int index)
        {
            return players[index];
        }

        public void AddNewPlayer(Player player)
        {
            players.Add(player);
            Global.instance.playerBuilders[index,player.index] = null;
        }
    }
}