using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using BattlescapeLogic;
using BattlescapeUI;

public class MouseoveredButtonUnitScript : MonoBehaviour, IPointerEnterHandler
{
    UnitCreator myUnit;

    public static event Action<UnitCreator> OnUnitHovered;


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
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        OnUnitHovered(myUnit);
    }
}
