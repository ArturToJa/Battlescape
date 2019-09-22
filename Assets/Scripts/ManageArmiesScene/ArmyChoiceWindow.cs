using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class ArmyChoiceWindow : MonoBehaviour
{

    // This class contains everything about the window in which we choose an army in ManageArmy mode.

    [SerializeField] GameObject HeroChoicer;
    [SerializeField] Transform ExiSaves;
    [SerializeField] GameObject savePrefab;
    [SerializeField] GameObject NewArmyWindow;
    WindowSetter windowSetter;

    void Start()
    {
        windowSetter = FindObjectOfType<WindowSetter>();
       // ReCreateSaves();
    }
    public void GoBack()
    {
        FindObjectOfType<LevelLoader>().CommandLoadScene("_MENU");
    }

    public void CreateNewArmy()
    {
        windowSetter.currentScreen = HeroChoicer;
        SaveLoadManager.Instance.currentSaveName = null;
        NewArmyWindow.SetActive(true);
        gameObject.SetActive(false);

    }
    public void AcceptNewArmyBuild()
    {
        HeroChoicer.SetActive(true);
        StartCoroutine(CloseWindow(NewArmyWindow));
    }
    public void ChooseCurrentArmy()
    {
        windowSetter.currentScreen = HeroChoicer;
        SaveLoadManager.Instance.LoadPlayerArmy();
        SaveLoadManager.Instance.RecreateUnitsList();
        HeroChoicer.SetActive(true);
        HeroChoicer.GetComponent<HeroChoiceScreenScript>().LoadHero(Unit.FindUnitByID(SaveLoadManager.Instance.playerArmy.heroID));
        //HeroNames.SetHeroName(TurnManager.Instance.PlayerToMove);
        StartCoroutine(CloseWindow(gameObject));
        foreach (Transform child in ArmyBuilder.Instance.HeroesChoice.transform)
        {
           ArmyBuilder.Instance.SetHeroPortrait(child.gameObject, (int)SaveLoadManager.Instance.Race);
        }        
    }  

    IEnumerator CloseWindow(GameObject Window)
    {
        while (Window.GetComponent<CanvasGroup>().alpha > 0.1f)
        {
            UIManager.SmoothlyTransitionActivity(Window.gameObject, false, 0.1f);
            yield return null;
        }
        Window.GetComponent<CanvasGroup>().alpha = 0;
    }

}
