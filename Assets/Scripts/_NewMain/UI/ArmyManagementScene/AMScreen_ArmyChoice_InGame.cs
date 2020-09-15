using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BattlescapeLogic;
using UnityEngine.UI;

namespace BattlescapeUI
{
    public class AMScreen_ArmyChoice_InGame : AMScreen_ArmyChoice
    {
        [SerializeField] Button goBackButton;
        [SerializeField] Button acceptButton;
        [SerializeField] Text chosenArmyText;

        void Start()
        {
            SpawnSaves();
            goBackButton.onClick.AddListener(Back);
            acceptButton.onClick.AddListener(ChooseArmy);
        }

        void Update()
        {
            acceptButton.gameObject.SetActive(!string.IsNullOrEmpty(chosenSaveName));
            chosenArmyText.text = chosenSaveName;
        }

        protected override void ChooseArmy()
        {
            Global.instance.armySavingManager.LoadArmy(Global.instance.armySavingManager.armySavePath + "/" + chosenSaveName + "." + Global.instance.armySavingManager.saveExtension);
            Player currentPlayer = FindObjectOfType<VERY_POORLY_WRITTEN_CLASS>().LoadPlayerToGame();
            transform.parent.gameObject.SetActive(false);
        }

        public void Back()
        {
            Global.instance.armySavingManager.ResetCurrentSaveToNull();
            Networking.instance.SendCommandToLoadScene("_MENU");
        }
    }
}