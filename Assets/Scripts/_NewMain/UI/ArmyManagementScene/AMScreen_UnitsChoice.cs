using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BattlescapeLogic;

namespace BattlescapeUI
{
    public class AMScreen_UnitsChoice : ArmyManagementScreen
    {
        ArmyBuilder armyBuilder;
        [SerializeField] Transform possibleUnits;
        [SerializeField] LeftUnitsList leftUnitsList;
        public Transform ownedUnitsTransform;
        [SerializeField] UnitStatShower statShower;
        [SerializeField] Text remainingGoldText;        

        public override void OnChoice()
        {
            base.OnChoice();
            UnitStatShower.currentInstance = statShower;
            armyBuilder = new ArmyBuilder(Global.instance.armySavingManager.currentSave.armySize, leftUnitsList.buttonPrefab, possibleUnits, ownedUnitsTransform);
            leftUnitsList.CreateButtons();
            armyBuilder.LoadPreexistingArmy(Global.instance.armySavingManager.currentSave.GetUnitCreators());
            SetRemainingGoldTextTo(armyBuilder.currentMoney);
            forwardButton.gameObject.SetActive(ownedUnitsTransform.childCount > 0);
        }

        public void OnUnitButtonPressed(Button button)
        {
            if (button.transform.parent == possibleUnits)
            {
                armyBuilder.TryAddUnit(button);                
            }
            else
            {
                armyBuilder.TryRemoveUnit(button);
            }
            forwardButton.gameObject.SetActive(ownedUnitsTransform.childCount > 0);
            SetRemainingGoldTextTo(armyBuilder.currentMoney);
        }

       

        void SetRemainingGoldTextTo(int value)
        {
            remainingGoldText.text = value.ToString();
        }

       


    }
}