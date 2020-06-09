using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BattlescapeLogic;
using BattlescapeUI;

public class ArmyBuilder : MonoBehaviour
{
    public static ArmyBuilder instance { get; private set; }

    // This class deals with modyfing Armies in Manage Armies mode (adding and removing units to your army to later save it
    public GameObject HeroesChoice;
    [SerializeField] GameObject raceScreen;
    [SerializeField] GameObject armyBuildingScreen;
    [SerializeField] Transform possibleUnits;
    public Transform ownedUnitsTransform;
    Dictionary<UnitCreator,UnitButtonScript> ownedUnits;
    public UnitCreator heroCreator { get; set; }
    int startingMoney;
    int currentMoney;
    [SerializeField] Text remainingGoldText;
    [SerializeField] GameObject heroChoiceScreen;
    public GameObject raceOK;
    public Text RaceNameText;
    public Text RaceDescriptionText;
    WindowSetter windowSetter;
    [SerializeField] LeftUnitsList leftUnitsList;
    public UnitStatShower unitStatShower;
    public UnitStatShower heroStatShower;

    //THIS shouldnt need to exist but i would have to redo everything to change that so.... i guess it stays?
    Dictionary<string, int> unitsAlreadyInArmy;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogError("why");
        }
        startingMoney = SaveLoadManager.instance.currentSaveValue;
        currentMoney = startingMoney;
        windowSetter = FindObjectOfType<WindowSetter>();
        ownedUnits = new Dictionary<UnitCreator, UnitButtonScript>();
    }

    void Update()
    {
        if (SaveLoadManager.instance.playerArmy != null && SaveLoadManager.instance.playerArmy.unitIndecies != null && SaveLoadManager.instance.playerArmy.unitIndecies.Count > 0)
        {
            raceScreen.SetActive(false);
        }
        remainingGoldText.text = currentMoney.ToString();
        if (Input.GetKeyDown(KeyCode.Escape) && windowSetter.currentScreen == this.gameObject)
        {
            GoBackToHeroScreen();
        }
        raceOK.SetActive(SaveLoadManager.instance.race != Race.Neutral);
        if (armyBuildingScreen.activeSelf)
        {
            foreach (Transform unitButton in possibleUnits)
            {
                if (unitButton.GetSiblingIndex() > 0)
                {
                    unitButton.gameObject.SetActive(unitButton.GetComponent<UnitButtonScript>().unitCreator != null && unitButton.GetComponent<UnitButtonScript>().unitCreator.prefab.GetComponent<Unit>().race == SaveLoadManager.instance.race);
                }
            }
        }

    }

    // This function below obviously ads or rremoves units to the Army and more specifically to our UnitsList in SaveLoadManager (which later SaveLoadManager saves into a save file 
    // ( so it is not adding/removing to any battlefield or anything).


    public void InputHeroName(string text)
    {
        SaveLoadManager.instance.heroName = text;
    }

    public void OnButtonPressed(Button button)
    {
        UnitCreator unit = button.GetComponent<UnitButtonScript>().unitCreator;
        if (button.transform.parent == possibleUnits)
        {
            if (CanBeLegallyAdded(unit))
            {
                //This has to be manually these two cause you dont want the second one done on each AddUnit (not when loading army).
                AddUnit(unit);
                SaveLoadManager.instance.unitsList.Add(unit);
            }
        }
        else
        {
            RemoveUnit(unit);
        }
    }

    bool CanBeLegallyAdded(UnitCreator unitCreator)
    {
        return currentMoney >= unitCreator.GetCost() && SaveLoadManager.instance.GetNumberOfUnits(unitCreator) < unitCreator.GetLimit();
    }

    public void AddUnit(UnitCreator unit)
    {
        Debug.Log(ownedUnits.ContainsKey(unit));
        if (ownedUnits.ContainsKey(unit) == false)
        {
            GameObject temp = Instantiate(leftUnitsList.buttonPrefab, ownedUnitsTransform);
            temp.name = unit.name;
            temp.GetComponent<UnitButtonScript>().OnCreation(unit);
            ownedUnits.Add(unit, temp.GetComponent<UnitButtonScript>()); 
        }
        ownedUnits[unit].amount++;
        currentMoney -= unit.GetCost();            
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
                DestroyImmediate(ownedUnits[unit].gameObject);
            }
            else
            {
                Destroy(ownedUnits[unit].gameObject);
            }
            ownedUnits.Remove(unit);
        }
        currentMoney += unit.GetCost();
        SaveLoadManager.instance.unitsList.Remove(unit);
    }

    public void AddHero(UnitCreator hero)
    {
        heroCreator = hero;
    }

    public void GoBackToHeroScreen()
    {
        windowSetter.currentScreen = heroChoiceScreen;
        StartCoroutine(BackToHeroCoroutine());
    }

    private IEnumerator BackToHeroCoroutine()
    {
        FindObjectOfType<HeroChoiceScreenScript>().amIActive = true;
        while (armyBuildingScreen.GetComponent<CanvasGroup>().alpha > 0.1f)
        {
            UIManager.SmoothlyTransitionActivity(gameObject, false, 0.1f);
            yield return null;
        }
        foreach (Transform item in ownedUnitsTransform)
        {
            Destroy(item.gameObject, 0.1f);
        }
        heroCreator = null;
        currentMoney = startingMoney;
        armyBuildingScreen.GetComponent<CanvasGroup>().alpha = 0;
        armyBuildingScreen.SetActive(false);
    }

    public bool IsHero()
    {
        return heroCreator != null;
    }
    public void LoadArmy(List<UnitCreator> army)
    {
        leftUnitsList.CreateButtons();
        foreach (UnitCreator item in army)
        {
            for (int i = 1; i < possibleUnits.childCount; i++)
            {
                if (possibleUnits.GetChild(i).GetComponent<UnitButtonScript>().unitCreator == item)
                {                    
                    possibleUnits.GetChild(i).GetComponent<UnitButtonScript>().AddButtonToRightList();
                }
            }
        }
    }
}
