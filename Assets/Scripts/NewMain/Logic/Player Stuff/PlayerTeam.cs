using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace BattlescapeLogic
{
    public class PlayerTeam : MonoBehaviour
    {
        public PlayerTeam(int index, int numberOfPlayers)
        {
            Index = index;
            Players = new List<Player>();
            for (int i = 0; i < numberOfPlayers; i++)
            {
              //  Players.Add(new Player()); - ?????
            }
        }

        public int Index;
        public List<Player> Players;



        
        Player GetPlayerByIndex(int index)
        {
            return Players[index];
        }
    }
}

