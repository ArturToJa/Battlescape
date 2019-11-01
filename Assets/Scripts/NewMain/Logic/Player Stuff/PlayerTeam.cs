using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattlescapeLogic
{
    public class PlayerTeam : INewTurn
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
        }

        public void OnNewTurn()
        {
            foreach(Player player in players)
            {
                player.OnNewTurn();
            }
        }
    }
}