using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BattlescapeLogic;

public class ArmyBuilder : MonoBehaviour
{
    public static ArmyBuilder Instance { get; private set; }

    // This class deals with modyfing Armies in Manage Armies mode (adding and removing units to your army to later save it
    public GameObject HeroesChoice;
    [SerializeField] GameObject FactionScreen;
    [SerializeField] GameObject ABScreen;
    public Transform DeploymentPanel;
    [SerializeField] Transform LeftUnits;
    public Transform RightUnits;
    public Unit Hero;
    public Button pressedButton;
    int startingMoney;
    [SerializeField] int currentMoney;
    [SerializeField] Text RemainingGoldText;
    public List<Unit> UnitsList;

    [SerializeField] Renderer Pedestal;
    [SerializeField] bool isVisual;
    [SerializeField] GameObject HeroChoicer;
    [SerializeField] GameObject FactionOK;
    public Text FactionNameText;
    public Text FactionDescriptionText;
    WindowSetter windowSetter;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogError("why");
        }
        startingMoney = SaveLoadManager.Instance.currentSaveValue;
        currentMoney = startingMoney;
        UnitsList = new List<Unit>();
        windowSetter = FindObjectOfType<WindowSetter>();
    }

    void Update()
    {
        if (SaveLoadManager.Instance.playerArmy != null && SaveLoadManager.Instance.playerArmy.unitIDs != null && SaveLoadManager.Instance.playerArmy.unitIDs.Count > 0)
        {
            FactionScreen.SetActive(false);
        }
        RemainingGoldText.text = currentMoney.ToString();
        if (Input.GetKeyDown(KeyCode.Escape) && windowSetter.currentScreen == this.gameObject)
        {
            GoBackToHeroScreen();
        }
        FactionOK.SetActive(SaveLoadManager.Instance.Race != Faction.Neutral);
        if (ABScreen.activeSelf)
        {
            foreach (Transform unitButton in LeftUnits)
            {
                if (unitButton.GetSiblingIndex() > 0)
                {
                    unitButton.gameObject.SetActive(unitButton.GetComponent<UnitButtonScript>().thisUnit.faction == SaveLoadManager.Instance.Race);
                }
            }
        }

    }

    // This function below obviously ads or rremoves units to the Army and more specifically to our UnitsList in SaveLoadManager (which later SaveLoadManager saves into a save file 
    // ( so it is not adding/removing to any battlefield or anything).


    public void InputHeroName(string text)
    {
        SaveLoadManager.Instance.HeroName = text;
    }

    public void AddOrRemoveUnit(bool isReal)
    {
        if (pressedButton.transform.parent == LeftUnits)
        {

            if (currentMoney >= pressedButton.GetComponent<UnitButtonScript>().thisUnit.Cost)
            {

                if (RightUnits.Find(pressedButton.name) != null)
                {
                    if (RightUnits.Find(pressedButton.name).GetComponent<UnitButtonScript>().GetAmount() < RightUnits.Find(pressedButton.name).GetComponent<UnitButtonScript>().thisUnit.LimitFor50)
                    {
                        RightUnits.Find(pressedButton.name).GetComponent<UnitButtonScript>().IncrementAmount();
                        currentMoney -= pressedButton.GetComponent<UnitButtonScript>().thisUnit.Cost;
                        AddPressedButtonToUnitList();
                        if (isReal)
                        {
                            SaveLoadManager.Instance.UnitsList.Add(pressedButton.GetComponent<UnitButtonScript>().thisUnit);
                        }
                    }

                }
                else
                {
                    var temp = Instantiate(pressedButton, RightUnits);
                    temp.name = pressedButton.name;
                    currentMoney -= pressedButton.GetComponent<UnitButtonScript>().thisUnit.Cost;
                    AddPressedButtonToUnitList();
                    if (isReal)
                    {
                        SaveLoadManager.Instance.UnitsList.Add(pressedButton.GetComponent<UnitButtonScript>().thisUnit);
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
            currentMoney += pressedButton.GetComponent<UnitButtonScript>().thisUnit.Cost;

            if (RightUnits.Find(pressedButton.name).GetComponent<UnitButtonScript>().GetAmount() > 1)
            {
                RightUnits.Find(pressedButton.name).GetComponent<UnitButtonScript>().DecrementAmount();
                RemovePressedButtonToUnitList();
                SaveLoadManager.Instance.UnitsList.Remove(pressedButton.GetComponent<UnitButtonScript>().thisUnit);
            }
            else
            {
                if (Application.isEditor)
                {
                    SaveLoadManager.Instance.UnitsList.Remove(pressedButton.GetComponent<UnitButtonScript>().thisUnit);
                    RemovePressedButtonToUnitList();
                    DestroyImmediate(pressedButton.gameObject);


                }
                else
                {
                    SaveLoadManager.Instance.UnitsList.Remove(pressedButton.GetComponent<UnitButtonScript>().thisUnit);
                    RemovePressedButtonToUnitList();
                    Destroy(pressedButton.gameObject);
                }
            }

        }


    }

    void RemovePressedButtonToUnitList()
    {
        UnitsList.Remove(pressedButton.GetComponent<UnitButtonScript>().thisUnit);
    }

    void AddPressedButtonToUnitList()
    {
        UnitsList.Add(pressedButton.GetComponent<UnitButtonScript>().thisUnit);
    }

    public void AddHero(Unit hero)
    {
        Hero = hero;
    }
    
    public void GoBackToHeroScreen()
    {
        windowSetter.currentScreen = HeroChoicer;
        StartCoroutine(BackToHeroCoroutine());
    }

    private IEnumerator BackToHeroCoroutine()
    {
        FindObjectOfType<HeroChoiceScreenScript>().amIActive = true;
        while (ABScreen.GetComponent<CanvasGroup>().alpha > 0.1f)
        {
            UIManager.SmoothlyTransitionActivity(gameObject, false, 0.1f);
            yield return null;
        }
        foreach (Transform item in RightUnits)
        {
            Destroy(item.gameObject, 0.1f);
        }
        Hero = null;
        currentMoney = startingMoney;
        ABScreen.GetComponent<CanvasGroup>().alpha = 0;
        ABScreen.SetActive(false);
    }

    public bool IsHero()
    {
        return Hero != null;
    }
    public void LoadArmy(List<Unit> army)
    {
        foreach (Unit item in army)
        {
            for (int i = 1; i < LeftUnits.childCount; i++)
            {
                if (LeftUnits.GetChild(i).GetComponent<UnitButtonScript>().thisUnit == item)
                {
                    LeftUnits.GetChild(i).GetComponent<UnitButtonScript>().AddButtonToRightList();
                }
            }
        }
    }

    public void ClickHuman()
    {
        SaveLoadManager.Instance.Race = Faction.Human;
        foreach (Transform child in HeroesChoice.transform)
        {
            SetHeroPortrait(child.gameObject, 0);
        }
    }

    public void ClickElves()
    {
        SaveLoadManager.Instance.Race = Faction.Elves;
        foreach (Transform child in HeroesChoice.transform)
        {
            SetHeroPortrait(child.gameObject, 1);
        }
    }

    public void SetHeroPortrait(GameObject portraitFrame, int raceID)
    {
        foreach (Transform child in portraitFrame.transform)
        {
            child.gameObject.SetActive(false);
        }
        portraitFrame.transform.GetChild(raceID).gameObject.SetActive(true);
    }
}
