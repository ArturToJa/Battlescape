using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattlescapeLogic
{
    //This is more like 'OnMouseDoesSomething Manager xD as it deals with whatever happens on mouse clicks and hovers.
    //Pls rename it to your liking ;)
    // As I can't work without SOME kind of a manager script knowing the rules and deciding over stuff, I'm making it that way. If you have a better idea, please refactor it.
    public class MouseManager: TurnChangeObject
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
        EnemyTooltipHandler rightClickTooltip;
        UIHitChanceInformation hitChanceInfoUI;

        public event System.Action<Unit> OnUnitSelection;



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
                        SelectUnit(clickedUnit);
                    }
                }
                else if (IsLegalToDeclareAttack(selectedUnit, clickedUnit))
                {
                    Networking.instance.SendCommandToStartAttack(selectedUnit, clickedUnit);
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

        public void SelectUnit(Unit unit)
        {            
            unitSelector.SelectUnit(unit);
            OnUnitSelection(unit);
        }



        public bool CanSelect(Unit unit)
        {
            return (/*Some ability-checker so that no ability is beiong used HERE && */ PlayerInput.instance.isInputBlocked == false && unit.owner.IsCurrentLocalPlayer() && GameRound.instance.currentPhase != TurnPhases.Enemy && GameRound.instance.currentPhase != TurnPhases.None);
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
                GameRound.instance.currentPhase == TurnPhases.Attack
                && attacker != null
                && defender != null
                && attacker.owner.IsCurrentLocalPlayer()
                && attacker.owner == GameRound.instance.currentPlayer
                && attacker.CanStillAttack()
                && attacker.IsInAttackRange(defender.transform.position)
                && attacker.HasClearView(defender.transform.position)
                && attacker.IsEnemyOf(defender);
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
                && selectedUnit.owner == GameRound.instance.currentPlayer
                && GameRound.instance.currentPhase == TurnPhases.Movement
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
                        if (selectedUnit.IsEnemyOf(unit) && Helper.WouldBeInAttackRange(selectedUnit, hoveredTile, unit.transform.position))
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
                CursorController.instance.OnUnitHovered(hoveredUnit);
            }
            else if (hoveredObject is Tile)
            {
                Tile hoveredTile = hoveredObject as Tile;
                CursorController.instance.OnTileHovered(hoveredTile);
            }
        }



        public void OnMouseLeftClick(IMouseTargetable hoveredObject)
        {
            if (AbstractActiveAbility.currentlyUsedAbility != null)
            {
                if (AbstractActiveAbility.currentlyUsedAbility.IsLegalTarget(hoveredObject))
                {
                    AbstractActiveAbility.currentlyUsedAbility.target = hoveredObject;
                    AbstractActiveAbility.currentlyUsedAbility.BaseActivate();
                }               
            }
            else if (hoveredObject is Unit)
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

        public override void OnNewRound()
        {
            return;
        }

        public override void OnNewTurn()
        {
            return;
        }

        public override void OnNewPhase()
        {
            unitSelector.DeselectUnit();
        }
    }
}