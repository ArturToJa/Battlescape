using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattlescapeLogic
{
    public class Player
    {
        public Player(int _index, PlayerColour _colour, string _playerName, Faction _race, PlayerType _type, PlayerTeam _team)
        {
            index = _index;
            colour = _colour;
            playerName = _playerName;
            race = _race;
            type = _type;
            team = _team;
            playerScore = 0;
            playerUnits = new List<Unit>();
        }

        public readonly int index;
        public PlayerTeam team { get; set; }
        public string playerName { get; set; }
        public Faction race { get; set; }
        public readonly PlayerType type;
        public readonly PlayerColour colour;
        public readonly List<Unit> playerUnits;
        public int playerScore { get; private set; }

        void AddNewUnit(Unit newUnit)
        {
            playerUnits.Add(newUnit);
        }
        void AddPoints(int points)
        {
            playerScore += points;
        }
        Unit GetUnitByIndex(int index)
        {
            foreach (Unit unit in playerUnits)
            {
                if (unit.index == index)
                {
                    return unit;
                }
            }
            Debug.LogError("NO UNIT FOUND!");
            return null;
        }
    }
}

