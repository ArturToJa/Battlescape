using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattlescapeLogic
{
    [System.Serializable]
    public class ArmySave
    {

        public string saveName { get; set; }

        //these are just how these prefabs are called in Resources folder.
        public List<string> unitPrefabPaths { get; private set; }
        public string heroPrefabPath { get; private set; }

        //its the name a player named his hero, not a class of a hero
        public string heroName;

        //race is an enum so....
        int race;

        //maybe we want more army types in the future - like bigger or smaller armies? In points. They should know what 'format' they are for.
        public int armySize = 25;

        //Maybe we want AI-only armies, that would be e.g. bigger thn the technical 'size' in points in which they compete - maybe like a 30-35 point army for AI to fight player's 25 point armies.
        //Players should deffinitely not be allowed to use these :D tho.
        public bool isAIOnly;

        public ArmySave()
        {
            unitPrefabPaths = new List<string>();
            race = (int)Race.Neutral;
        }



        public void AddUnit(UnitCreator unit)
        {
            unitPrefabPaths.Add(unit.name);
        }

        public void RemoveUnit(UnitCreator unit)
        {
            unitPrefabPaths.Remove(unit.name);
        }

        public void SetHero(UnitCreator hero)
        {
            heroPrefabPath = hero.name;
        }

        public void SetRace(Race _race)
        {
            race = (int)_race;
        }
        public Race GetRace()
        {
            return (Race)race;
        }

        public List<UnitCreator> GetUnitCreators()
        {
            UnitCreator[] allUC = Resources.LoadAll<UnitCreator>("UnitCreator");
            List<UnitCreator> list = new List<UnitCreator>();
            foreach (string unitName in unitPrefabPaths)
            {
                foreach (UnitCreator unit in allUC)
                {
                    if (unit.name == unitName)
                    {
                        list.Add(unit);
                    }
                }                
            }
            return list;
        }
        public UnitCreator GetHero()
        {
            UnitCreator[] allUC = Resources.LoadAll<UnitCreator>("UnitCreator");
            foreach (UnitCreator unit in allUC)
            {
                if (unit.name == heroPrefabPath)
                {
                    return unit;
                }
            }
            Debug.LogError("Hero name not found in all Unit Creator names");
            return null;
        }

        public int GetQuantityOfUnit(UnitCreator unit)
        {
            int returnInt = 0;
            foreach (string possible in unitPrefabPaths)
            {
                if (unit.name == possible)
                {
                    returnInt++;
                }
            }
            return returnInt;
        }
    }
}