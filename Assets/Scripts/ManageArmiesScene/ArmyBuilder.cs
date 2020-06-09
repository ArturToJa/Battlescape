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
    [SerializeField] Transform leftUnits;
    public Transform RightUnits;
    public UnitCreator heroCreator { get; set; }
    public Button pressedButton { get; set; }
    int startingMoney;
    int currentMoney;
    [SerializeField] Text remainingGoldText;
    public List<UnitCreator> UnitsList;
    [SerializeField] GameObject heroChoiceScreen;
    public GameObject raceOK;
    public Text RaceNameText;
    public Text RaceDescriptionText;
    WindowSetter windowSetter;
    [SerializeField] LeftUnitsList leftUnitsList;
    public UnitStatShower unitStatShower;
    public UnitStatShower heroStatShower;

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
        UnitsList = new List<UnitCreator>();
        windowSetter = FindObjectOfType<WindowSetter>();
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
            foreach (Transform unitButton in leftUnits)
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

    public void AddOrRemoveUnit(bool isReal)
    {
        if (pressedButton.transform.parent == leftUnits)
        {

            if (currentMoney >= pressedButton.GetComponent<UnitButtonScript>().unitCreator.prefab.GetComponent<Unit>().statistics.cost)
            {

                if (RightUnits.Find(pressedButton.name) != null)
                {
                    if (RightUnits.Find(pressedButton.name).GetComponent<UnitButtonScript>().GetAmount() < RightUnits.Find(pressedButton.name).GetComponent<UnitButtonScript>().unitCreator.prefab.GetComponent<Unit>().statistics.limit)
                    {
                        RightUnits.Find(pressedButton.name).GetComponent<UnitButtonScript>().IncrementAmount();
                        currentMoney -= pressedButton.GetComponent<UnitButtonScript>().unitCreator.prefab.GetComponent<Unit>().statistics.cost;
                        AddPressedButtonToUnitList();
                        if (isReal)
                        {
                            SaveLoadManager.instance.unitsList.Add(pressedButton.GetComponent<UnitButtonScript>().unitCreator);
                        }
                    }

                }
                else
                {
                    GameObject temp = Instantiate(pressedButton.gameObject, RightUnits);
                    temp.name = pressedButton.name;
                    temp.GetComponent<UnitButtonScript>().OnCreation(pressedButton.GetComponent<UnitButtonScript>().unitCreator);
                    currentMoney -= pressedButton.GetComponent<UnitButtonScript>().unitCreator.prefab.GetComponent<Unit>().statistics.cost;
                    AddPressedButtonToUnitList();
                    if (isReal)
                    {
                        SaveLoadManager.instance.unitsList.Add(pressedButton.GetComponent<UnitButtonScript>().unitCreator);
                    }
                }
            }
            else
            {
                return;
            }
        }

        else
        {
            currentMoney += pressedButton.GetComponent<UnitButtonScript>().unitCreator.prefab.GetComponent<Unit>().statistics.cost;

            if (RightUnits.Find(pressedButton.name).GetComponent<UnitButtonScript>().GetAmount() > 1)
            {
                RightUnits.Find(pressedButton.name).GetComponent<UnitButtonScript>().DecrementAmount();
                RemovePressedButtonToUnitList();
                SaveLoadManager.instance.unitsList.Remove(pressedButton.GetComponent<UnitButtonScript>().unitCreator);
            }
            else
            {
                if (Application.isEditor)
                {
                    SaveLoadManager.instance.unitsList.Remove(pressedButton.GetComponent<UnitButtonScript>().unitCreator);
                    RemovePressedButtonToUnitList();
                    DestroyImmediate(pressedButton.gameObject);


                }
                else
                {
                    SaveLoadManager.instance.unitsList.Remove(pressedButton.GetComponent<UnitButtonScript>().unitCreator);
                    RemovePressedButtonToUnitList();
                    Destroy(pressedButton.gameObject);
                }
            }

        }


    }

    //In general this should probably take the 
    public void OnButtonPressed()
    {

    }

    void RemovePressedButtonToUnitList()
    {
        UnitsList.Remove(pressedButton.GetComponent<UnitButtonScript>().unitCreator);
    }

    void AddPressedButtonToUnitList()
    {
        UnitsList.Add(pressedButton.GetComponent<UnitButtonScript>().unitCreator);
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
        foreach (Transform item in RightUnits)
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
        foreach (UnitCreator item in army)
        {
            for (int i = 1; i < leftUnits.childCount; i++)
            {
                if (leftUnits.GetChild(i).GetComponent<UnitButtonScript>().unitCreator == item)
                {
                    leftUnits.GetChild(i).GetComponent<UnitButtonScript>().AddButtonToRightList();
                }
            }
        }
        leftUnitsList.CreateButtons();
    }    
}
