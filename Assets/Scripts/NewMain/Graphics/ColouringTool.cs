using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BattlescapeLogic;
using System;

namespace BattlescapeGraphics
{
    public static class ColouringTool
    {
        public static void ColourUnitsThatStillCanMoveOrAttack()
        {
            Player currentPlayer = Global.instance.playerTeams[TurnManager.Instance.PlayerHavingTurn].players[0];
            foreach (Unit unit in currentPlayer.playerUnits)
            {
                if (unit.CanAttackOrMoveNow())
                {
                    ColourObject(unit, Color.green);
                }
            }
        }

        public static void UncolourAllUnits()
        {
            foreach (PlayerTeam team in Global.instance.playerTeams)
            {
                foreach (Player player in team.players)
                {
                    foreach (Unit unit in player.playerUnits)
                    {
                        ColourObject(unit, Color.white);
                    }
                }
            }
        }

        public static void ColourLegalTilesFor(Unit unit)
        {
            UncolourAllTiles();
            if (TurnManager.Instance.CurrentPhase == TurnPhases.Movement)
            {
                var temp = Pathfinder.instance.GetAllLegalTilesFor(unit);
                foreach (Tile tile in temp)
                {
                    if (tile.IsProtectedByEnemyOf(unit))
                    {
                        ColourObject(tile, Color.red);
                    }
                    else
                    {
                        ColourObject(tile, Color.cyan);
                    }
                }
            }
            else if (TurnManager.Instance.CurrentPhase == TurnPhases.Attack)
            {
                foreach (Tile tile in Map.Board)
                {
                    if (tile.myUnit != null && tile.myUnit.owner.team != unit.currentPosition.myUnit.owner.team && unit.IsInAttackRange(tile.transform.position) && unit.CanStillAttack())
                    {
                        ColourObject(tile, Color.red);
                    }
                }
            }
           
        }

        public static void UncolourAllTiles()
        {
            foreach (Tile tile in Map.Board)
            {
                ColourObject(tile, Color.white);
            }

        }

        public static void ColourUnitAsAllyOrEnemy(Unit unit)
        {
            if (unit.owner.IsCurrentLocalPlayer())
            {
                ColourObject(unit, Color.green);
            }
            else
            {
                ColourObject(unit, Color.red);
            }
        }                       

        public static void ColourObject(MonoBehaviour target, Color color)
        {
            Renderer[] rs = target.GetComponentsInChildren<Renderer>();
            foreach (Renderer r in rs)
            {
                Material m = r.material;
                m.color = color;
                r.material = m;
            }
        }                  
    }
}