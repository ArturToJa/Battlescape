using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

namespace BattlescapeLogic
{
    public class ArmySavingManager
    {
        [SerializeField] string _armySavePath;
        public string armySavePath
        {
            get
            {
                return _armySavePath;
            }
            private set
            {
                _armySavePath = value;
            }
        }
        [SerializeField] Sprite[] RaceSymbols;
        ArmySave currentSave;


        static ArmySavingManager _instance;
        public static ArmySavingManager instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ArmySavingManager();
                }
                return _instance;
            }
        }

        public void OnClickSaveArmy()
        {
            SerializationManager.instance.Save(currentSave.saveName, armySavePath, currentSave);
        }

        public void OnClickLoadArmy(string path)
        {
            currentSave = (ArmySave)SerializationManager.instance.Load(path);
        }

        public List<string> GetAllSaveNames(string path)
        {
            List<string> saveNames = new List<string>();
            DirectoryInfo dir = new DirectoryInfo(path);
            if (dir.Exists == false)
            {
                return null;
            }
            FileInfo[] info = dir.GetFiles("*.lemur");
            foreach (FileInfo f in info)
            {
                saveNames.Add(Path.GetFileNameWithoutExtension(f.FullName));
            }
            return saveNames;
        }

        public bool DeleteIfNotComplete(ArmySave save)
        {            
            if (save.GetRace() == Race.Neutral)
            {
                Debug.Log("deleted - no Race!");
                File.Delete(armySavePath + "/" + save.saveName + ".save");
                return true;
            }
            if (save.GetHero() == null)
            {
                Debug.Log("deleted - nonexistant hero path");
                File.Delete(armySavePath + "/" + save.saveName + ".save");
                return true;
            }
            return false;
        }

        //public bool IsWrongRace(ArmySave save)
        //{            
        //    if (save.GetRace() != Global.instance.GetLocalPlayer().race)                
        //    {
        //        Debug.Log("Skipped");
        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}

        public Sprite GetRaceSprite(Race race)
        {
            return RaceSymbols[(int)race];
        }

    }
}