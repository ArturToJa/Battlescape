﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattlescapeLogic
{
    public class PlayerTeam
    {
        public PlayerTeam(int index, int numberOfPlayers)
        {
            this.index = index;
            Players = new List<Player>(numberOfPlayers);
        }

        public int index;
        public List<Player> Players;

        Player GetPlayerByIndex(int index)
        {
            return Players[index];
        }

        public void AddNewPlayer(Player player)
        {
            Players.Add(player);
        }
    }
}