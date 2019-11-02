using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIStatsValues : MonoBehaviour
{
    [SerializeField] bool RealOne;
    //[SerializeField] int Attack;
    //[SerializeField] int Defence;
    //[SerializeField] int Movement;
    //[SerializeField] int Health;
    //[SerializeField] int AttackRange;
    //[SerializeField] int Value;
    //[SerializeField] int PoisonCounters;
    UnitScript unit;

    [SerializeField] Text attackText;
    [SerializeField] Text defenceText;
    [SerializeField] Text movementText;
    [SerializeField] Text healthText;
    [SerializeField] Text shootingRangeText;
    [SerializeField] Text valueText;
    //[SerializeField] Text poisonCountersText;

    void Update()
    {
        if (RealOne == false && EnemyTooltipHandler.isOn)
        {
            return;
        }
        if (SetSelectedUnit())
        {
            AdjustTextValues();
        }
    }

    bool SetSelectedUnit()
    {
        if (RealOne)
        {
            if (MouseManager.Instance.SelectedUnit == null)
            {
                return false;
            }
            unit = MouseManager.Instance.SelectedUnit;
        }
        else
        {
            if (MouseManager.Instance.MouseoveredUnit == null)
            {
                return false;
            }
            unit = MouseManager.Instance.MouseoveredUnit;
        }



        //Attack = unit.statistics.baseAttack + ;
        //Defence = unit.statistics.GetCurrentDefence();
        //Health = unit.statistics.healthPoints;
        //Movement = unit.GetComponent<UnitScript>().GetCurrentMoveSpeed(true);
        //AttackRange = unit.statistics.GetCurrentAttackRange();
        //Value = unit.statistics.cost;
        //PoisonCounters = unit.PoisonCounter;
   
        
        
        return true;

    }

    void AdjustTextValues()
    {
        SetValuesAndColours(unit.statistics.bonusAttack, unit.statistics.baseAttack, attackText);
        SetValuesAndColours(unit.statistics.bonusDefence, unit.statistics.baseDefence, defenceText);
        SetValuesAndColours(unit.statistics.bonusMaxMovementPoints, unit.statistics.baseMaxMovementPoints, movementText);
        SetValuesAndColours(unit.statistics.bonusAttackRange, unit.statistics.baseAttackRange, shootingRangeText);
        healthText.text = unit.statistics.healthPoints.ToString();                
        if (unit.statistics.cost > 0)
        {
            valueText.transform.parent.parent.gameObject.SetActive(true);
            valueText.text = unit.statistics.cost.ToString();
        }
        else
        {
            valueText.transform.parent.parent.gameObject.SetActive(false);
        }

        //if (PoisonCounters > 0)
        //{
        //    poisonCountersText.transform.parent.parent.gameObject.SetActive(true);
        //    poisonCountersText.text = PoisonCounters.ToString();
        //}
        //else
        //{
        //    poisonCountersText.transform.parent.parent.gameObject.SetActive(false);
        //}
    }

    void SetValuesAndColours(int bonusValue, int baseValue, Text t)
    {
        t.text = baseValue.ToString();
        if (bonusValue > 0)
        {
            t.text += "<size=15> + </size><color=green>" + bonusValue + "</color>";
        }
        if (bonusValue < 0)
        {
            t.text += "<size=15> - </size><color=red>" + bonusValue + "</color>";
        }        
    }
}
