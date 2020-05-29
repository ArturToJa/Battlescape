using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BattlescapeLogic;

public class ClickableHeroUIScript : MonoBehaviour
{

    [SerializeField] ChosenHero CH;
    [SerializeField] ArmyBuilder ab;
    public UnitCreator myHero;
    public List<Sprite> BasicSprites;
    public List<Sprite> HighlightSprites;


    private void Start()
    {
        SaveLoadManager.instance.OnRaceChosenAction += OnRaceChosen;
        if (ab == null)
        {
            ab = FindObjectOfType<ArmyBuilder>();
        }
        CH = FindObjectOfType<ChosenHero>();
        GetComponent<Button>().onClick.AddListener(TaskOnClick);        
    }

    public void TaskOnClick()
    {
        ArmyBuilder.instance.pressedButton = this.GetComponent<Button>();
        ArmyBuilder.instance.AddHero(myHero);
        CH.ChoseHero(GetComponent<Image>().sprite);
    }

    public void OnRaceChosen()
    {
        if (myHero == null || SaveLoadManager.instance.race != myHero.prefab.GetComponent<Unit>().race)
        {
            transform.parent.gameObject.SetActive(false);
        }
    }

    void OnDestroy()
    {
        SaveLoadManager.instance.OnRaceChosenAction -= OnRaceChosen;
    }
}
