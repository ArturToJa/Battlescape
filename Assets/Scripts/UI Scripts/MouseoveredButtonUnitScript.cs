using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using BattlescapeLogic;

public class MouseoveredButtonUnitScript : MonoBehaviour, IPointerEnterHandler
{
    [SerializeField] UnitStatShower myUSS;
    

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (this.GetComponent<UnitButtonScript>() != null)
        {
            myUSS.currUnit = this.GetComponent<UnitButtonScript>().unitCreator.prefab.GetComponent<Unit>();
        }
        else if (this.GetComponent<ClickableHeroUIScript>() != null)
        {
            myUSS.currUnit = this.GetComponent<ClickableHeroUIScript>().myHero.prefab.GetComponent<Hero>();
        }
        myUSS.UpdateInfos();
    }
}
