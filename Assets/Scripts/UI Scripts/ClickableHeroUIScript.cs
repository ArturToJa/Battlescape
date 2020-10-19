using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BattlescapeLogic;
using BattlescapeUI;

public class ClickableHeroUIScript : MonoBehaviour
{
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
        GetComponent<Button>().onClick.AddListener(TaskOnClick);
    }


    public void TaskOnClick()
    {
        Global.instance.armySavingManager.currentSave.SetHero(unitCreator);
        FindObjectOfType<AMScreen_HeroChoice>().OnHeroChoice(myImage.sprite);
    }
}
