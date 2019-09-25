using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattlescapeLogic
{
    public class PlayerTeam
    {
        public PlayerTeam(int index, int numberOfPlayers)
        {
            Index = index;
            Players = new List<Player>(numberOfPlayers);
        }

        public int Index;
        public List<Player> Players;

        Player GetPlayerByIndex(int index)
        {
            return Players[index];
        }

        void AddNewPlayer(Player player)
        {
            Players.Add(player);
        }
    }
}