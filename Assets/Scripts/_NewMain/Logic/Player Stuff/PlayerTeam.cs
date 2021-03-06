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
            foreach (PlayerBuilder playerBuilder in Global.instance.playerBuilders)
            {
                if (playerBuilder.index == player.index && playerBuilder.team.index == player.team.index)
                {
                    Global.instance.playerBuilders.Remove(playerBuilder);
                    break;
                }
            }          
        }

        public bool HasHumanPlayer()
        {
            foreach (Player player in players)
            {
                if (player.type != PlayerType.AI)
                {
                    return true;
                }
            }
            return false;
        }

        public bool HasCurrentLocalPlayer()
        {
            foreach (Player player in players)
            {
                if (player.IsCurrentLocalPlayer())
                {
                    return true;
                }
            }
            return false;
        }

        public bool HasLost()
        {
            foreach (Player player in players)
            {
                if (player.hasLost == false)
                {
                    return false;
                }
            }
            return true;
        }
    }
}