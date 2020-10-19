using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

namespace BattlescapeLogic
{
    [System.Serializable]
    public class ArmySavingManager
    {
        [SerializeField] string _saveExtension;
        public string saveExtension
        {
            get
            {
                return _saveExtension;
            }
            private set
            {
                _saveExtension = value;
            }
        }
        [SerializeField] string _armySavePath;
        public string armySavePath
        {
            get
            {               
                return Application.persistentDataPath + "/" + _armySavePath;
            }
            private set
            {
                _armySavePath = value;
            }
        }
        [SerializeField] Sprite[] raceSymbols;
        public ArmySave currentSave { get; private set; } 

        public void CreateNewArmy(string name)
        {
            currentSave = new ArmySave
            {
                saveName = name,
                
            };            
        }

        public void ResetCurrentSaveToNull()
        {
            currentSave = null;
        }

        public void OnClickNewArmy()
        {
            currentSave = null;
        }

        public void OnClickSaveArmy()
        {
            SerializationManager.instance.Save(currentSave.saveName, armySavePath, currentSave, saveExtension);
            currentSave = null;
        }

        public void LoadArmy(string path)
        {
            currentSave = GetArmy(path);
        }

        public ArmySave GetArmy(string path)
        {
            return (ArmySave)SerializationManager.instance.Load(path);
        }

        public List<string> GetAllSaveNames(string path)
        {
            List<string> saveNames = new List<string>();
            DirectoryInfo dir = new DirectoryInfo(path);
            if (dir.Exists == false)
            {
                return null;
            }
            FileInfo[] info = dir.GetFiles("*." + saveExtension);
            foreach (FileInfo f in info)
            {
                saveNames.Add(Path.GetFileNameWithoutExtension(f.FullName));
            }
            return saveNames;
        }

        public bool IsCorrectSave(ArmySave save)
        {
            if (save == null)
            {
                return false;
            }
            if (save.GetRace() == Race.Neutral)
            {
                Debug.LogError("deleted - no Race!");
                //File.Delete(armySavePath + "/" + save.saveName + "." + saveExtension);
                return false;
            }
            if (save.GetHero() == null)
            {
                Debug.LogError("deleted - nonexistant hero path");
                //File.Delete(armySavePath + "/" + save.saveName + "." + saveExtension);
                return false;
            }
            return true;
        }

        public Sprite GetRaceSprite(Race race)
        {
            return raceSymbols[(int)race];
        }

        public bool HasArmyOfRace(Race race)
        {
            var allSaves = GetAllSaveNames(armySavePath);
            if (allSaves != null)
            {
                foreach (var saveName in allSaves)
                {
                    ArmySave save = GetArmy(saveName);
                    if (save.GetRace() == Race.Neutral)
                    {
                        Debug.Log("deleted");
                        SerializationManager.instance.Delete(armySavePath + "/" + saveName + "." + Global.instance.armySavingManager.saveExtension);
                        continue;
                    }
                    if (save.GetRace() == race)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

    }
}