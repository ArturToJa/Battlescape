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
        if (ab == null)
        {
            ab = FindObjectOfType<ArmyBuilder>();
        }
        CH = FindObjectOfType<ChosenHero>();
        GetComponent<Button>().onClick.AddListener(TaskOnClick);
    }

    public void TaskOnClick()
    {
        ArmyBuilder.Instance.pressedButton = this.GetComponent<Button>();
        ArmyBuilder.Instance.AddHero(myHero);
        CH.ChoseHero(GetComponent<Image>().sprite);
    }
}
