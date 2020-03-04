using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BattlescapeLogic;

//Note - this is totally misnamed - not ENEMY, just RIGHT CLICK TOOLTIP :)
public class EnemyTooltipHandler : MonoBehaviour
{
    [SerializeField] Text Title;    

    public event Action <Unit> OnRightclickTooltipOn;
    public static EnemyTooltipHandler instance;

    void Awake()
    {
        instance = this;
    }


    public void SetOnFor(Unit unit)
    {
        OnRightclickTooltipOn(unit);
        UIManager.InstantlyTransitionActivity(this.gameObject, true);
        transform.SetPositionAndRotation(Input.mousePosition + new Vector3(-90, 40, 0), Quaternion.identity);
        Helper.CheckIfInBoundries(transform);
        Title.text = unit.unitName;
    }
    
    public void TurnOff()
    {
        UIManager.InstantlyTransitionActivity(this.gameObject, false);
    }
}
