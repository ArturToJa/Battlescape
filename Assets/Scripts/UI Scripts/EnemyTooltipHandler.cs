using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BattlescapeLogic;

//Note - this is totally misnamed - not ENEMY, just RIGHT CLICK TOOLTIP :)
public class EnemyTooltipHandler : MonoBehaviour
{
    [SerializeField] Text title;    

    public event Action <Unit> OnRightclickTooltipOn;
    public static EnemyTooltipHandler instance;

    void Awake()
    {
        instance = this;
        TurnOff();
    }


    public void SetOnFor(Unit unit)
    {
        OnRightclickTooltipOn(unit);
        UIManager.InstantlyTransitionActivity(this.gameObject, true);
        transform.SetPositionAndRotation(Input.mousePosition + new Vector3(-90, 40, 0), Quaternion.identity);
        Helper.CheckIfInBoundries(transform);

        title.text = unit.info.unitName;
        if (unit is Hero)
        {
            title.text += " " + (unit as Hero).heroName;
        }
    }
    
    public void TurnOff()
    {
        UIManager.InstantlyTransitionActivity(this.gameObject, false);
    }
}
