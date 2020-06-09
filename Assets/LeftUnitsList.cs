﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BattlescapeLogic;

namespace BattlescapeUI
{
    public class LeftUnitsList : MonoBehaviour
    {
        [SerializeField] GameObject _buttonPrefab;  
        public GameObject buttonPrefab
        {
            get
            {
                return _buttonPrefab;
            }
            private set
            {
                _buttonPrefab = value;
            }
        }

        public void CreateButtons()
        {
            foreach (UnitCreator creator in SaveLoadManager.instance.allUnitCreators)
            {
                if (creator.IsCompatible(SaveLoadManager.instance.race) && creator.IsHero() == false)
                {
                    CreateButton(creator);
                }
            }
        }

        void CreateButton(UnitCreator creator)
        {
            GameObject buttonObject = Instantiate<GameObject>(buttonPrefab, this.transform);
            UnitButtonScript button = buttonObject.GetComponent<UnitButtonScript>();
            button.OnCreation(creator);
        }
    }
}