using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BattlescapeUI;

namespace BattlescapeLogic
{
    public class ArmyBuilder
    {
        int startingMoney;
        public int currentMoney { get; set; }
        Dictionary<UnitCreator, UnitButtonScript> ownedUnits;
        GameObject buttonPrefab;
        Transform possibleUnits;
        Transform targetTransform;

        public ArmyBuilder(int _startingMoney, GameObject _buttonPrefab, Transform _possibleUnits, Transform _targetTransform)
        {
            startingMoney = _startingMoney;
            currentMoney = startingMoney;
            ownedUnits = new Dictionary<UnitCreator, UnitButtonScript>();
            buttonPrefab = _buttonPrefab;
            targetTransform = _targetTransform;
            possibleUnits = _possibleUnits;
        }       

        public void TryRemoveUnit(Button button)
        {
            UnitCreator unit = button.GetComponent<UnitButtonScript>().unitCreator;
            RemoveUnit(unit);
        }

        public void TryAddUnit(Button button)
        {
            UnitCreator unit = button.GetComponent<UnitButtonScript>().unitCreator;
            if (CanBeLegallyAdded(unit))
            {
                //This has to be manually these two cause you dont want the second one done on each AddUnit (not when loading army).
                AddUnit(unit);
                Global.instance.armySavingManager.currentSave.AddUnit(unit);
            }
        }

        bool CanBeLegallyAdded(UnitCreator unitCreator)
        {
            return currentMoney >= unitCreator.GetCost() && Global.instance.armySavingManager.currentSave.GetQuantityOfUnit(unitCreator) < unitCreator.GetLimit();
        }

        public void AddUnit(UnitCreator unit)
        {
            if (ownedUnits.ContainsKey(unit) == false)
            {
                GameObject button = AddButton(unit);
                ownedUnits.Add(unit, button.GetComponent<UnitButtonScript>());
            }
            ownedUnits[unit].amount++;
            currentMoney -= unit.GetCost();
        }

        public GameObject AddButton(UnitCreator unit)
        {
            GameObject unitButton = Object.Instantiate(buttonPrefab, targetTransform);
            unitButton.name = unit.name;
            unitButton.GetComponent<UnitButtonScript>().OnCreation(unit);
            return unitButton;
        }

        void RemoveUnit(UnitCreator unit)
        {
            if (ownedUnits[unit].amount > 1)
            {
                ownedUnits[unit].amount--;
            }
            else
            {
                if (Application.isEditor)
                {
                    Object.DestroyImmediate(ownedUnits[unit].gameObject);
                }
                else
                {
                    Object.Destroy(ownedUnits[unit].gameObject);
                }
                ownedUnits.Remove(unit);
            }
            currentMoney += unit.GetCost();
            Global.instance.armySavingManager.currentSave.RemoveUnit(unit);
        }

        public void LoadPreexistingArmy(List<UnitCreator> army)
        {
            foreach (UnitCreator item in army)
            {
                for (int i = 1; i < possibleUnits.childCount; i++)
                {
                    if (possibleUnits.GetChild(i).GetComponent<UnitButtonScript>().unitCreator == item)
                    {
                        AddUnit(possibleUnits.GetChild(i).GetComponent<UnitButtonScript>().unitCreator);
                    }
                }
            }
        }
    }
}