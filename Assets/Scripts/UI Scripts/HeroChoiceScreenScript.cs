using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BattlescapeLogic;

public class HeroChoiceScreenScript : MonoBehaviour
{
    [SerializeField] ChosenHero chosenHero;
    [SerializeField] GameObject OKButton;
    [SerializeField] GameObject ArmyBuildingScreen;
    public bool amIActive = true;
    WindowSetter windowSetter;

    private void Start()
    {
        windowSetter = FindObjectOfType<WindowSetter>();
    }

    private void Update()
    {
        OKButton.SetActive(ArmyBuilder.Instance.IsHero());
        UIManager.SmoothlyTransitionActivity(this.gameObject, amIActive, 0.1f);

    }

    public void Okay()
    {
        windowSetter.currentScreen = ArmyBuildingScreen;
        amIActive = false;
        StartCoroutine(ABFadeIn());
        chosenHero.theHero = null;
        ArmyBuilder.Instance.LoadArmy(GetUnitsFromCreators(SaveLoadManager.Instance.playerArmy.unitIndecies));

    }

    List<Unit> GetUnitsFromCreators(List<int> unitCreators)
    {
        List<Unit> returnList = new List<Unit>();
        foreach (int unitCreator in unitCreators)
        {            
            returnList.Add(SaveLoadManager.Instance.GetUnitCreatorFromIndex(unitCreator).prefab.GetComponent<Unit>());
        }    
        return returnList;
    }

public void NextPlayer()
{
    amIActive = true;
}

public IEnumerator ABFadeIn()
{
    if (ArmyBuildingScreen.activeSelf == false)
    {
        ArmyBuildingScreen.SetActive(true);
    }
    while (ArmyBuildingScreen.GetComponent<CanvasGroup>().alpha < 1f)
    {
        UIManager.SmoothlyTransitionActivity(ArmyBuildingScreen, true, 0.01f);
        yield return null;
    }
}

public void LoadHero(int heroIndex)
{
    foreach (var item in FindObjectsOfType<ClickableHeroUIScript>())
    {
        if (item.myHero.index == heroIndex)
        {
            item.TaskOnClick();
        }
    }
}
}
