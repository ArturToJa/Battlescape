using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattlescapeLogic
{
    public class GameTurn
    {
        public void OnClick()
        {
            NewTurn();
        }

        private void NewTurn()
        {
            foreach(PlayerTeam team in Global.instance.playerTeams)
            {
                team.OnNewTurn();
            }
        }
    }
}

