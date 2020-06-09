using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using BattlescapeLogic;

public class MouseoveredButtonUnitScript : MonoBehaviour, IPointerEnterHandler
{
    UnitStatShower myUSS;
    Pedestal pedestal;
    UnitCreator myUnit;


    void Start()
    {
        pedestal = FindObjectOfType<Pedestal>();
        if (this.GetComponent<UnitButtonScript>() != null)
        {
            myUnit = this.GetComponent<UnitButtonScript>().unitCreator;
            myUSS = ArmyBuilder.instance.unitStatShower;
        }
        else
        {
            myUnit = this.GetComponent<ClickableHeroUIScript>().unitCreator;
            myUSS = ArmyBuilder.instance.heroStatShower;
        }
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        myUSS.currUnit = myUnit.prefab.GetComponent<Unit>();
        myUSS.UpdateInfos();
        pedestal.ShowUnit(myUnit.prefab);
    }
}
