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
    UIStatsValues stats
    {
        get
        {
            return GetComponentInChildren<UIStatsValues>();
            //this is inefficient but whatevs;
            //its old code anyways;
        }
    }
    

    public void SetOnFor(Unit unit)
    {
        UIManager.InstantlyTransitionActivity(this.gameObject, true);
        transform.SetPositionAndRotation(Input.mousePosition + new Vector3(-90, 40, 0), Quaternion.identity);
        Helper.CheckIfInBoundries(transform);
        stats.AdjustTextValuesFor(unit);
    }
    
    public void TurnOff()
    {
        UIManager.InstantlyTransitionActivity(this.gameObject, false);
    }
}
