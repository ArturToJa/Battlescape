using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BattlescapeLogic;
using UnityEngine.UI;

namespace BattlescapeUI
{
    public class AMScreen_ArmyChoice_Management : AMScreen_ArmyChoice
    {
        [SerializeField] Button deleteButton;
        [SerializeField] Button newArmyButton;
        [SerializeField] GameObject newArmyScreen;
        [SerializeField] Text saveNameText;

        void Update()
        {
            deleteButton.gameObject.SetActive(!string.IsNullOrEmpty(chosenSaveName));
            forwardButton.gameObject.SetActive(!string.IsNullOrEmpty(chosenSaveName));
            if (string.IsNullOrEmpty(chosenSaveName))
            {
                saveNameText.text = string.Empty;
            }
            else
            {
                saveNameText.text = chosenSaveName;
            }
        }

        public override void OnSetup()
        {
            base.OnSetup();
            deleteButton.onClick.AddListener(Delete);
            newArmyButton.onClick.AddListener(NewArmy);
        }

        void Delete()
        {
            //This should be rewritten but its way out of scope of what im doing now.
            FindObjectOfType<DeleteSaves>().OpenWindow();
        }

        void NewArmy()
        {
            newArmyScreen.SetActive(true);
            Global.instance.armySavingManager.OnClickNewArmy();
        }

        protected override void ChooseArmy()
        {            
            Global.instance.armySavingManager.LoadArmy(Global.instance.armySavingManager.armySavePath + "/" + chosenSaveName + "." + Global.instance.armySavingManager.saveExtension);
        }
    }
}