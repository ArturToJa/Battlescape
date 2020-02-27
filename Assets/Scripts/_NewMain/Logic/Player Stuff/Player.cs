using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattlescapeLogic
{
    public enum PlayerType { Local, AI, Network }

    public enum PlayerColour { Green, Red }

    public enum Faction { Human, Elves, Neutral }

    public class Player 
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
            isObserver = builder.isObserver;
        }

        public readonly int index;
        public PlayerTeam team { get; set; }
        public string playerName { get; set; }
        public Faction race { get; set; }
        public readonly PlayerType type;
        public readonly PlayerColour colour;
        public readonly List<Unit> playerUnits;
        public int playerScore { get; private set; }
        public bool isObserver { get; private set; }

        void AddNewUnit(Unit newUnit)
        {
            playerUnits.Add(newUnit);
        }
        public void AddPoints(int points)
        {
            playerScore += points;
        }
        public Unit GetUnitByIndex(int index)
        {
            foreach (Unit unit in playerUnits)
            {
                /*if (unit.index == index)
                {
                    return unit;
                }*/
            }
            Debug.LogError("NO UNIT FOUND!");
            return null;
        }

        public void AddUnit(Unit myUnit)
        {
            playerUnits.Add(myUnit);
            myUnit.owner = this;
        }

        /// <summary>
        /// Returns true only, if it is the player currently 'active' on this computer.
        /// </summary>
        /// <returns></returns>
        public bool IsCurrentLocalPlayer()
        {
            if (type != PlayerType.Local)
            {
                return false;
            }
            if (Global.instance.matchType == MatchTypes.HotSeat && this != GameRound.instance.currentPlayer)
            {
                return false;
            }
            return true;
            //meaning: the player is local and if the match type is HotSeat, the player is the guy who's time it is to move;
        }

        /// <summary>
        /// Returns true, if a player has units, that can still legally make the basic action in this phase (move in Movement Phase, attack in Attack Phase).
        /// </summary>
        public bool HasAttacksOrMovesLeft()
        {
            foreach (Unit unit in playerUnits)
            {
                if (unit.CanAttackOrMoveNow())
                {
                    return true;
                }
            }
            return false;
        }

    }
}




