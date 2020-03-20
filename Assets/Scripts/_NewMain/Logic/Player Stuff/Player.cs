using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattlescapeLogic
{
    public enum PlayerType { Local, AI, Network }

    public enum PlayerColour { Green, Red }

    public enum Race { Human, Elves, Neutral }

    public class Player : IActiveEntity
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
        public Race race { get; set; }
        public readonly PlayerType type;
        public readonly PlayerColour colour;
        public readonly List<Unit> playerUnits;
        public int playerScore { get; private set; }
        public bool isObserver { get; private set; }
        public Unit selectedUnit { get; private set; }

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

        public void OnRightClick(IMouseTargetable target)
        {
            if (target is Unit)
            {
                var targetUnit = target as Unit;
                if (targetUnit.IsAlive())
                {
                    EnemyTooltipHandler.instance.SetOnFor(targetUnit);
                }
            }
        }

        public void OnLeftClick(IMouseTargetable target)
        {
            if (target is Unit)
            {
                Unit targetUnit = target as Unit;
                if (targetUnit.CanBeSelected())
                {
                    SelectUnit(targetUnit);
                }
            }
        }

        /// <summary>
        /// Selects a random unit that still has actions this phase
        /// </summary>
        public void SelectRandomUnit()
        {
            if (type != PlayerType.Local)
            {
                return;
            }
            List<Unit> PossibleUnits = new List<Unit>();
            foreach (Unit unit in playerUnits)
            {
                if (unit.CanAttackOrMoveNow())
                {
                    PossibleUnits.Add(unit);
                }
            }

            if (PossibleUnits.Count > 0)
            {
                SelectUnit(PossibleUnits[UnityEngine.Random.Range(0, PossibleUnits.Count)]);
            }
            else
            {
                PopupTextController.AddPopupText("No more units ot act!", PopupTypes.Info);
            }
        }

        public void SelectUnit(Unit unit)
        {
            if (unit.owner != this)
            {
                Debug.LogError("Tried to select unit that's not mine!");
                return;
            }
            selectedUnit = unit;
            unit.OnSelection();
            Global.instance.currentEntity = unit;
        }

        public void DeselectUnit()
        {
            selectedUnit.OnDeselection();
            selectedUnit = null;
        }

        public void OnCursorOver(IMouseTargetable target)
        {
            if (target is Unit)
            {
                var targetUnit = target as Unit;
                if (targetUnit.CanBeSelected())
                {
                    Cursor.instance.OnSelectableHovered();
                }
            }
            if (target is Tile)
            {
                Cursor.instance.SetToDefault();
            }
        }

    }
}




