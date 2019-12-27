using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattlescapeLogic
{
    //This is more like 'OnMouseDoesSomething Manager xD as it deals with whatever happens on mouse clicks and hovers.
    //Pls rename it to your liking ;)
    // As I can't work without SOME kind of a manager script knowing the rules and deciding over stuff, I'm making it that way. If you have a better idea, please refactor it.
    public class MouseManager
    {
        static MouseManager _instance;
        public static MouseManager instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new MouseManager();
                }
                return _instance;
            }
        }

        public Unit selectedUnit { get; set; }
        public UnitSelector unitSelector { get; private set; }
        UIStatsValues selectedUnitStats;
        EnemyTooltipHandler rightClickTooltip;
        UIHitChanceInformation hitChanceInfoUI;




        private MouseManager()
        {
            unitSelector = new UnitSelector();
            SetUI();
        }

        void SetUI()
        {
            hitChanceInfoUI = GameObject.FindObjectOfType<UIHitChanceInformation>();
            hitChanceInfoUI.TurnOff();
            rightClickTooltip = GameObject.FindObjectOfType<EnemyTooltipHandler>();
            rightClickTooltip.TurnOff();
            var temp = GameObject.FindObjectsOfType<UIStatsValues>();
            foreach (var item in temp)
            {
                if (item.isRightClickTooltip == false)
                {
                    selectedUnitStats = item;
                }
            }
        }

        //This function should be only called in one place IMO;
        public void OnTileClicked(Tile clickedTile)
        {
            //ability here
            if (CanSelectedUnitMoveTo(clickedTile))
            {
                Networking.instance.SendCommandToMove(selectedUnit, clickedTile);
            }
        }


        void OnUnitLeftClicked(Unit clickedUnit)
        {
            if (clickedUnit.IsAlive())
            {
                //Here: ability!
                if (CanSelect(clickedUnit))
                {

                    //Selection stuff
                    if (selectedUnit == clickedUnit)
                    {
                        unitSelector.DeselectUnit();
                    }
                    else
                    {
                        unitSelector.SelectUnit(clickedUnit);
                        selectedUnitStats.AdjustTextValuesFor(clickedUnit);
                    }
                }
                else if (IsLegalToDeclareAttack(selectedUnit, clickedUnit))
                {
                    Networking.instance.SendCommandToAttack(selectedUnit, clickedUnit);
                    selectedUnit.statistics.numberOfAttacks--;
                }
            }
        }

        void OnUnitRightClicked(Unit clickedUnit)
        {
            if (clickedUnit.IsAlive())
            {
                rightClickTooltip.SetOnFor(clickedUnit);
            }
        }



        public bool CanSelect(Unit unit)
        {
            return (/*Some ability-checker so that no ability is beiong used HERE && */ PlayerInput.instance.isInputBlocked == false && unit.owner.IsCurrentLocalPlayer() && TurnManager.Instance.CurrentPhase != TurnPhases.Enemy && TurnManager.Instance.CurrentPhase != TurnPhases.None);
        }

        /// <summary>
        /// Note that this only deals with NORMAL attacks, not with ability-attacks, combat-quitting etc.
        /// </summary>
        /// <param name="attacker"></param>
        /// <param name="defender"></param>
        /// <returns></returns>
        public bool IsLegalToDeclareAttack(Unit attacker, Unit defender)
        {
            return
                TurnManager.Instance.CurrentPhase == TurnPhases.Attack
                && attacker != null
                && defender != null
                && attacker.owner.IsCurrentLocalPlayer()
                && attacker.CanStillAttack()
                && attacker.IsInAttackRange(defender.transform.position)
                && attacker.owner.team != defender.owner.team;
        }

        /// <summary>
        /// Returns if it is legal for selected unit in this exact moment to move to this exact tile
        /// </summary>
        /// <param name="unitToMove"></param>
        /// <param name="destination"></param>
        /// <returns></returns>
        public bool CanSelectedUnitMoveTo(Tile destination)
        {
            if (destination == null)
            {
                Debug.LogError("Destination is null!");
            }
            return
                selectedUnit != null
                && destination != null
                && selectedUnit.owner.IsCurrentLocalPlayer()
                && TurnManager.Instance.CurrentPhase == TurnPhases.Movement
                && selectedUnit.CanStillMove()
                && Pathfinder.instance.IsLegalTileForUnit(destination, selectedUnit)
                && PlayerInput.instance.isInputBlocked == false;
        }

        public void OnMouseHoverEnter(IMouseTargetable hoveredObject)
        {
            if (hoveredObject is Unit)
            {
                Unit hoveredUnit = hoveredObject as Unit;
                if (hoveredUnit.IsAlive())
                {
                    BattlescapeGraphics.ColouringTool.ColourUnitAsAllyOrEnemy(hoveredUnit);
                    if (IsLegalToDeclareAttack(selectedUnit, hoveredUnit))
                    {
                        hitChanceInfoUI.TurnOnFor(selectedUnit, hoveredUnit);
                    }
                }
            }
            if (hoveredObject is Tile)
            {
                Tile hoveredTile = hoveredObject as Tile;
                if (CanSelectedUnitMoveTo(hoveredTile))
                {
                    foreach (Unit unit in Global.instance.GetAllUnits())
                    {
                        if (unit.owner.team != selectedUnit.owner.team && Helper.WouldBeInAttackRange(selectedUnit, hoveredTile, unit.transform.position))
                        {
                            BattlescapeGraphics.ColouringTool.ColourObject(unit, Color.red);
                        }
                    }
                }                                
            }
        }

        public void OnMouseHoverExit(IMouseTargetable hoveredObject)
        {
            if (hoveredObject is Unit)
            {
                Unit hoveredUnit = hoveredObject as Unit;
                BattlescapeGraphics.ColouringTool.ColourObject(hoveredUnit, Color.white);
                hitChanceInfoUI.TurnOff();
            }
            if (hoveredObject is Tile)
            {
                foreach (Unit unit in Global.instance.GetAllUnits())
                {
                    BattlescapeGraphics.ColouringTool.ColourObject(unit, Color.white);
                }
            }
        }

        public void OnMouseHoverContinue(IMouseTargetable hoveredObject)
        {
            if (hoveredObject is Unit)
            {
                Unit hoveredUnit = hoveredObject as Unit;
                CursorController.Instance.OnUnitHovered(hoveredUnit);
            }
            else if (hoveredObject is Tile)
            {
                Tile hoveredTile = hoveredObject as Tile;
                CursorController.Instance.OnTileHovered(hoveredTile);
            }
        }



        public void OnMouseLeftClick(IMouseTargetable hoveredObject)
        {
            if (hoveredObject is Unit)
            {
                Unit hoveredUnit = hoveredObject as Unit;
                OnUnitLeftClicked(hoveredUnit);
            }
            else if (hoveredObject is Tile)
            {
                Tile hoveredTile = hoveredObject as Tile;
                OnTileClicked(hoveredTile);
            }
        }

        public void OnMouseRightClick(IMouseTargetable hoveredObject)
        {
            if (hoveredObject is Unit)
            {
                Unit hoveredUnit = hoveredObject as Unit;
                OnUnitRightClicked(hoveredUnit);
            }
        }
    }
}