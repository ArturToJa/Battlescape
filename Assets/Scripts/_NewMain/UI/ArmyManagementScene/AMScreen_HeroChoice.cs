using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BattlescapeLogic;
using UnityEngine.UI;

namespace BattlescapeUI
{
    public class AMScreen_HeroChoice : ArmyManagementScreen
    {
        [SerializeField] GameObject heroButtonPrefab;
        [SerializeField] UnitStatShower statShower;
        [SerializeField] Transform legalHeroes;
        [SerializeField] Image chosenHero;
        [SerializeField] InputField heroNameInputField;
        [SerializeField] Button acceptNameButton;
        public string heroName { get; private set; }
        [SerializeField] Button acceptClassButton;
        [SerializeField] Pedestal pedestal;

        void Update()
        {
            acceptNameButton.gameObject.SetActive(string.IsNullOrEmpty(heroName) == false);
            acceptClassButton.gameObject.SetActive(Global.instance.armySavingManager.currentSave != null && (string.IsNullOrEmpty(Global.instance.armySavingManager.currentSave.heroPrefabPath) == false));
        }

        public override void OnSetup()
        {
            base.OnSetup();            
            forwardButton.onClick.AddListener(OnAcceptName);
        }
        public override void OnChoice()
        {
            base.OnChoice();
            GenerateLegalHeroButtons();
            UnitStatShower.currentInstance = statShower;
            if (IsClassAlreadyChosen())
            {
                SetToChosenHero();                
                if (IsNameAlreadyChosen())
                {
                    heroName = Global.instance.armySavingManager.currentSave.heroName;
                }
            }

        }

        private static bool IsNameAlreadyChosen()
        {
            return Global.instance.armySavingManager.currentSave.heroName != null;
        }

        void SetToChosenHero()
        {
            Hero hero = UnitCreator.FindUnitPrefabByName(Global.instance.armySavingManager.currentSave.heroPrefabPath).GetComponent<Hero>();
            OnHeroChoice(hero.avatarTransparent);            
        }

        private static bool IsClassAlreadyChosen()
        {
            return Global.instance.armySavingManager.currentSave.heroPrefabPath != null;
        }

        void GenerateLegalHeroButtons()
        {
            while (legalHeroes.childCount > 0)
            {
                if (Application.isEditor)
                {
                    DestroyImmediate(legalHeroes.GetChild(0).gameObject);
                }
                else
                {
                    Destroy(legalHeroes.GetChild(0).gameObject);
                }
            }
            foreach (UnitCreator creator in Resources.LoadAll<UnitCreator>("UnitCreator"))
            {
                if (creator.IsCompatible(Global.instance.armySavingManager.currentSave.GetRace()) && creator.IsHero())
                {
                    CreateButton(creator);
                }
            }
        }
        void CreateButton(UnitCreator creator)
        {
            GameObject buttonObject = Instantiate<GameObject>(heroButtonPrefab, legalHeroes.transform);
            ClickableHeroUIScript button = buttonObject.GetComponentInChildren<ClickableHeroUIScript>();
            button.OnCreation(creator);
        }

        public void OnHeroChoice(Sprite heroSprite)
        {
            chosenHero.sprite = heroSprite;
            chosenHero.color = Color.white;
        }

        public void OnEndEditHeroName(string _heroName)
        {
           heroName = _heroName;
        }

        void OnAcceptName()
        {
            Global.instance.armySavingManager.currentSave.heroName = heroName;
        }
    }
}