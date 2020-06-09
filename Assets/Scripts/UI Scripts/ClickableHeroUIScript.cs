using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BattlescapeLogic;
using System;

public class ClickableHeroUIScript : MonoBehaviour
{

    [SerializeField] ChosenHero CH;
    [SerializeField] ArmyBuilder ab;
    Image myImage;
    public UnitCreator unitCreator;

    public void OnCreation(UnitCreator creator)
    {
        myImage = GetComponent<Image>();
        myImage.sprite = creator.prefab.GetComponent<Hero>().avatarTransparent;
        SpriteState ss = new SpriteState();
        ss.highlightedSprite = creator.prefab.GetComponent<Hero>().avatarHighlightedTransparent;
        GetComponentInChildren<Button>().spriteState = ss;        
        unitCreator = creator;
        ab = FindObjectOfType<ArmyBuilder>();
        CH = FindObjectOfType<ChosenHero>();
        GetComponent<Button>().onClick.AddListener(TaskOnClick);
    }


    void Start()
    {
        
    }

    public void TaskOnClick()
    {
        ArmyBuilder.instance.pressedButton = this.GetComponent<Button>();
        ArmyBuilder.instance.AddHero(unitCreator);
        CH.ChoseHero(GetComponent<Image>().sprite);
    }
}
