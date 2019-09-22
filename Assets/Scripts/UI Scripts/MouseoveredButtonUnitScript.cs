using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseoveredButtonUnitScript : MonoBehaviour, IPointerEnterHandler
{
    [SerializeField] UnitStatShower myUSS;
    

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (this.GetComponent<UnitButtonScript>() != null)
        {
            myUSS.currUnit = this.GetComponent<UnitButtonScript>().thisUnit.thisUnitFirstPlayer.GetComponent<UnitScript>();
        }
        else if (this.GetComponent<ClickableHeroUIScript>() != null)
        {
            myUSS.currUnit = this.GetComponent<ClickableHeroUIScript>().myHero.thisUnitFirstPlayer.GetComponent<UnitScript>();
        }
        myUSS.UpdateInfos();
    }
}
