using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattlescapeLogic
{
    public enum PlayerType { Local, AI, Network }

    public enum PlayerColour { Green, Red }

    public enum Faction { Human, Elves, Neutral }

    public class Player : INewTurn
    {
        public Player(PlayerBuilder builder)
        {
            index = builder.index;
            colour = builder.colour;
            playerName = builder.playerName;
            race = builder.race;
            type = builder.type;
            team = builder.team;
            playerScore = 0;
            playerUnits = builder.playerUnits;
        }

        public readonly int index;
        public PlayerTeam team { get; set; }
        public string playerName { get; set; }
        public Faction race { get; set; }
        public readonly PlayerType type;
        public readonly PlayerColour colour;
        public readonly List<UnitScript> playerUnits;
        public int playerScore { get; private set; }

        void AddNewUnit(UnitScript newUnit)
        {
            playerUnits.Add(newUnit);
        }
        public void AddPoints(int points)
        {
            playerScore += points;
        }
        public Unit GetUnitByIndex(int index)
        {
            foreach (UnitScript unit in playerUnits)
            {
                /*if (unit.index == index)
                {
                    return unit;
                }*/
            }
            Debug.LogError("NO UNIT FOUND!");
            return null;
        }

        public void OnNewTurn()
        {
            foreach(UnitScript unit in playerUnits)
            {
                //unit.OnNewTurn();
            }
        }
    }
}




