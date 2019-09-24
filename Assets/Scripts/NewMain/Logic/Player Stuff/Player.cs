using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattlescapeLogic
{
    public class Player : MonoBehaviour
    {
        public Player(int index, PlayerColour colour, string name, Faction race, PlayerType type, PlayerTeam team)
        {
            Index = index;
            Colour = colour;
            Name = name;
            Race = race;
            Type = type;
            Team = team;
            PlayerScore = 0;
            PlayerUnits = new List<Unit>();
        }


        public readonly int Index;
        public PlayerTeam Team { get; set; }
        public string Name { get; set; }
        public Faction Race { get; set; }
        public readonly PlayerType Type;
        public readonly PlayerColour Colour;
        public readonly List<Unit> PlayerUnits;
        public int PlayerScore { get; private set; }




        void AddNewUnit(Unit newUnit)
        {
            PlayerUnits.Add(newUnit);
        }
        void AddPoints(int points)
        {
            PlayerScore += points;
        }
        Unit GetUnitByIndex(int index)
        {
            foreach (Unit unit in PlayerUnits)
            {
                if (unit.Index == index)
                {
                    return unit;
                }
            }
            Debug.LogError("NO UNIT FOUND!");
            return null;
        }
    }
}

