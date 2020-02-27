using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BattlescapeLogic;

public class UIStatsValues : MonoBehaviour
{
    [SerializeField] bool _isRightClickTooltip;
    public bool isRightClickTooltip
    {
        get
        {
            return _isRightClickTooltip;            
        }
        set
        {
            _isRightClickTooltip = value;
        }
    }

    [SerializeField] Text attackText;
    [SerializeField] Text defenceText;
    [SerializeField] Text movementText;
    [SerializeField] Text healthText;
    [SerializeField] Text shootingRangeText;
    [SerializeField] Text valueText;

    Unit myUnit;

    void Start()
    {
        AbstractBuff.OnBuffDestruction += OnBuffCreatedOrDestroyed;
        AbstractBuff.OnBuffCreation += OnBuffCreatedOrDestroyed;
        if (isRightClickTooltip == false)
        {
            MouseManager.instance.OnUnitSelection += AdjustTextValuesFor;
        }
        else
        {
            EnemyTooltipHandler.instance.OnRightclickTooltipOn += AdjustTextValuesFor;
        }
    }

    public void AdjustTextValuesFor(Unit unit)
    {
        myUnit = unit;
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
    }

    void SetValuesAndColours(int bonusValue, int baseValue, Text t)
    {
        t.text = baseValue.ToString();
        if (bonusValue > 0)
        {
            t.text += "<size=15>  </size><color=green>" + bonusValue + "</color>";
        }
        if (bonusValue < 0)
        {
            t.text += "<size=15>  </size><color=red>" + bonusValue + "</color>";
        }        
    }

    void OnBuffCreatedOrDestroyed(AbstractBuff buff)
    {
        if (buff.owner == myUnit)
        {
            AdjustTextValuesFor(myUnit);
        }
    }
}
