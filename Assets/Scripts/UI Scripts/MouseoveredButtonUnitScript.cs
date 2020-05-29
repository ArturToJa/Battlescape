using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using BattlescapeLogic;

public class MouseoveredButtonUnitScript : MonoBehaviour, IPointerEnterHandler
{
    [SerializeField] UnitStatShower myUSS;
    Pedestal pedestal;
    UnitCreator myUnit;


    void Start()
    {
        pedestal = FindObjectOfType<Pedestal>();
        if (this.GetComponent<UnitButtonScript>() != null)
        {
            myUnit = this.GetComponent<UnitButtonScript>().unitCreator;
        }
        else if (this.GetComponent<ClickableHeroUIScript>() != null)
        {
            myUnit = this.GetComponent<ClickableHeroUIScript>().myHero;
        }
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        myUSS.currUnit = myUnit.prefab.GetComponent<Unit>();
        myUSS.UpdateInfos();
        pedestal.ShowUnit(myUnit.prefab);
    }
}
