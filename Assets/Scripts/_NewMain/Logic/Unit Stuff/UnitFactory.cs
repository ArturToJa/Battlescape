using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattlescapeLogic
{
    public class UnitFactory
    {
        public static Unit GetUnitByIndex(int index)
        {
            foreach (PlayerTeam team in Global.instance.playerTeams)
            {
                foreach (Player player in team.players)
                {
                    foreach (Unit unit in player.playerUnits)
                    {
                        if (unit.index == index)
                        {
                            return unit;
                        }
                    }
                }
            }
            return null;
        }
    }
}