using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattlescapeLogic
{
    public class PlayerBuilder
    {
        

        public PlayerBuilder()
        {
            playerUnits = new List<Unit>();
        }
        public int index { get; set; }
        public string playerName { get; set; }
        public Race race { get; set; }
        public PlayerType type { get; set; }
        public PlayerColour colour { get; set; }
        public List<Unit> playerUnits { get; set; }
        public PlayerTeam team { get; set; }
        public bool isObserver { get; set; }
    }
}
