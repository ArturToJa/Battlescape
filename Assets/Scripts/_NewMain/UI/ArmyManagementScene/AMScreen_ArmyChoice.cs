using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BattlescapeLogic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace BattlescapeUI
{
    public abstract class AMScreen_ArmyChoice : ArmyManagementScreen
    {
        [SerializeField] GameObject savePrefab;
        [SerializeField] Transform existingSaves;
        public string chosenSaveName { get; set; }

        public override void OnSetup()
        {
            forwardButton.onClick.AddListener(ChooseArmy);
            base.OnSetup();      
        }
        public override void OnChoice()
        {
            base.OnChoice();
            SpawnSaves();
        }

        protected void SpawnSaves()
        {
            while (existingSaves.childCount > 0)
            {
                if (Application.isEditor)
                {
                    DestroyImmediate(existingSaves.GetChild(0).gameObject);
                }
                else
                {
                    Destroy(existingSaves.GetChild(0).gameObject);
                }
            }

            var saveList = Global.instance.armySavingManager.GetAllSaveNames(Global.instance.armySavingManager.armySavePath);
            if (saveList != null)
            {
                foreach (var saveName in saveList)
                {
                    GameObject armySave = Instantiate(savePrefab, existingSaves);
                    armySave.GetComponentInChildren<Text>().text = saveName;
                    ArmySave actualSaveData = (ArmySave)SerializationManager.instance.Load(Global.instance.armySavingManager.armySavePath + "/" + saveName + "." + Global.instance.armySavingManager.saveExtension);
                    if (Global.instance.armySavingManager.DeleteIfNotComplete(actualSaveData))
                    {
                        continue;
                    }
                    armySave.GetComponentsInChildren<Image>()[1].sprite = Global.instance.armySavingManager.GetRaceSprite(actualSaveData.GetRace());
                    //temp.GetComponentsInChildren<Image>()[2].sprite = RaceSymbols[(int)armyInfo.heroID];
                    armySave.name = saveName;
                }
            }
        }

        protected abstract void ChooseArmy();
    }
}