using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using BattlescapeLogic;
using BattlescapeUI;

public class MouseoveredButtonUnitScript : MonoBehaviour, IPointerEnterHandler
{
    Pedestal pedestal;
    UnitCreator myUnit;


    void Start()
    {
        if (GetComponent<UnitButtonScript>() != null)
        {
            myUnit = GetComponent<UnitButtonScript>().unitCreator;
        }
        else
        {
            myUnit = GetComponent<ClickableHeroUIScript>().unitCreator;
        }
        pedestal = FindObjectOfType<Pedestal>();        
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        UnitStatShower.UpdateUnitInfo(myUnit.prefab.GetComponent<Unit>());
        pedestal.ShowUnit(myUnit.prefab);
    }
}
