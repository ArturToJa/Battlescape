using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BattlescapeLogic;

namespace BattlescapeUI
{
    public class PossibleHeroesList : MonoBehaviour
    {
        [SerializeField] GameObject buttonBrefab;

        private void Start()
        {
            SaveLoadManager.instance.OnRaceChosenAction += CreateButtons;
        }

        public void CreateButtons()
        {
            while (transform.childCount > 0)
            {
                if (Application.isEditor)
                {
                    DestroyImmediate(transform.GetChild(0).gameObject);
                }
                else
                {
                    Destroy(transform.GetChild(0).gameObject);
                }
            }
            foreach (UnitCreator creator in SaveLoadManager.instance.allUnitCreators)
            {
                if (creator.IsCompatible(SaveLoadManager.instance.race) && creator.IsHero())
                {
                    CreateButton(creator);
                }
            }
        }

        void CreateButton(UnitCreator creator)
        {
            GameObject buttonObject = Instantiate<GameObject>(buttonBrefab, this.transform);
            ClickableHeroUIScript button = buttonObject.GetComponentInChildren<ClickableHeroUIScript>();
            button.OnCreation(creator);
        }

        private void OnDestroy()
        {
            SaveLoadManager.instance.OnRaceChosenAction -= CreateButtons;
        }
    }
}