﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BattlescapeLogic
{
    public class UnitSelector
    {


        SelectionIndicator selectionIndicator;



        public UnitSelector()
        {
            selectionIndicator = Object.FindObjectOfType<SelectionIndicator>();
        }

        public void SelectUnit(Unit unit)
        {
            selectionIndicator.SetActiveFor(unit);            
            MouseManager.instance.selectedUnit = unit;
            if (MouseManager.instance.selectedUnit.CanAttackOrMoveNow())
            {
                BattlescapeGraphics.ColouringTool.ColourLegalTilesFor(MouseManager.instance.selectedUnit);
            }
            if (Global.instance.IsCurrentPlayerAI() == false)
            {
                UIManager.UpdateAbilitiesPanel(UIManager.Instance.AbilitiesPanel, UIManager.Instance.AbilityPrefab, MouseManager.instance.selectedUnit);
            }
            if (Global.instance.IsCurrentPlayerAI() == false)
            {
                PlaySelectionSound();
            }
        }

        public void DeselectUnit()
        {
            BattlescapeGraphics.ColouringTool.UncolourAllTiles();
            selectionIndicator.SetInactive();
            MouseManager.instance.selectedUnit = null;
        }

        void PlaySelectionSound()
        {
            BattlescapeSound.SoundManager.instance.PlaySound(MouseManager.instance.selectedUnit.gameObject, BattlescapeSound.SoundManager.instance.selectionSound);
        }

        public void SelectNextAvailableUnit()
        {

            List<Unit> AllUnits = new List<Unit>(VictoryLossChecker.GetMyUnitList());
            List<Unit> PossibleUnits = new List<Unit>();
            foreach (Unit unit in AllUnits)
            {
                if (unit.CanAttackOrMoveNow())
                {
                    PossibleUnits.Add(unit);
                }
            }

            if (PossibleUnits.Count > 0)
            {
                SelectUnit(PossibleUnits[Random.Range(0, PossibleUnits.Count)]);
            }
            else
            {
                PopupTextController.AddPopupText("No more units ot act!", PopupTypes.Info);
            }
        }
    }
}