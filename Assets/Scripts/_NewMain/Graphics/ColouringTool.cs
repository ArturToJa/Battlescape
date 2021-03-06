﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BattlescapeLogic;
using System;

namespace BattlescapeGraphics
{
    public static class ColouringTool
    {
        readonly static float hoveringTilesBrightnessFactor = 1.5f;


        static ColouringTool()
        {
            Unit.OnUnitSelected += ColourLegalTilesFor;
            Unit.OnUnitDeselected += UncolourAllTiles;
        }
        public static void ColourUnitsThatStillCanMoveOrAttack()
        {
            Player currentPlayer = GameRound.instance.currentPlayer;
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
            if (GameRound.instance.currentPhase == TurnPhases.Movement)
            {
                var temp = Pathfinder.instance.GetAllLegalPositionsFor(unit);
                foreach (MultiTile position in temp)
                {
                    foreach (Tile tile in position)
                    {
                        if (tile.IsProtectedByEnemyOf(unit))
                        {
                            tile.highlighter.TurnOn(Global.instance.colours.red);
                        }
                        else
                        {
                            tile.highlighter.TurnOn(Global.instance.colours.yellow);
                        }
                    }
                }
            }
            else if (GameRound.instance.currentPhase == TurnPhases.Attack)
            {
                foreach (Tile tile in Global.instance.currentMap.board)
                {
                    if (tile.GetMyDamagableObject() != null && unit.attack.CanAttack(tile.GetMyDamagableObject()))
                    {
                        tile.highlighter.TurnOn(Global.instance.colours.red);
                    }
                }
            }
        }

        public static void UncolourAllTiles()
        {
            foreach (Tile tile in Global.instance.currentMap.board)
            {
                tile.highlighter.TurnOff();
            }
        }

        public static void ColourUnitAsAllyOrEnemyOf(Unit unit, Player player)
        {
            if (unit.GetMyOwner().team == player.team)
            {
                ColourObject(unit, Color.green);
            }
            else
            {
                ColourObject(unit, Color.red);
            }
        }

        public static void ColourObject(MonoBehaviour target, Color colour)
        {
            if (target is Tile)
            {
                Debug.LogWarning("thats not how you colour tiles. Try tile.highlighter.TurnOn(Color colour)");
            }

            Renderer[] rs = target.GetComponentsInChildren<Renderer>();
            foreach (Renderer r in rs)
            {
                Material m = r.material;
                m.color = colour;
                r.material = m;
            }
        }

        static Color GetColourOfBrighteness(Color colour, float factor)
        {
            return new Color(colour.r * factor, colour.g * factor, colour.b * factor, colour.a);
        }

        public static void OnPositionHovered(MultiTile position)
        {
            foreach (Tile tile in position)
            {
                tile.highlighter.TurnOn(GetColourOfBrighteness(tile.highlighter.GetComponent<Renderer>().material.color, hoveringTilesBrightnessFactor));
            }
        }

        public static void OnPositionUnhovered(MultiTile position)
        {
            foreach (Tile tile in position)
            {
                tile.highlighter.TurnOn(GetColourOfBrighteness(tile.highlighter.GetComponent<Renderer>().material.color, 1.0f / hoveringTilesBrightnessFactor));
            }
        }
    }
}