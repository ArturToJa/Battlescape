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
            Unit.OnUnitSelected += AdjustTextValuesFor;
        }
        else
        {
            EnemyTooltipHandler.instance.OnRightclickTooltipOn += AdjustTextValuesFor;
        }
    }

    public void AdjustTextValuesFor(Unit unit)
    {
        myUnit = unit;
        SetValuesAndColoursForCombatDependent(unit, unit.statistics.bonusAttack, unit.statistics.baseAttack, unit.statistics.GetCurrentMeleeAttack(), attackText);
        SetValuesAndColours(unit.statistics.bonusDefence, unit.statistics.baseDefence, defenceText);
        SetValuesAndColours(unit.statistics.bonusMaxMovementPoints, unit.statistics.baseMaxMovementPoints, movementText);
        SetValuesAndColoursForCombatDependent(unit, unit.statistics.attackRange.bonusAttackRange, unit.statistics.attackRange.baseAttackRange, unit.statistics.attackRange.GetCurrentAttackRangeInCombat(), shootingRangeText);
        
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

    void SetValuesAndColoursForCombatDependent(Unit unit, int bonusRange, int baseRange, int melee, Text t)
    {
        if (unit.IsInCombat())
        {
            SetValuesAndColoursForMelee(melee, bonusRange, baseRange, t);
        }
        else
        {
            SetValuesAndColours(bonusRange, baseRange, t);
        }
    }

    void SetValuesAndColoursForMelee(int melee, int bonusRange, int baseRange, Text t)
    {
        int range = bonusRange + baseRange;
        if (melee < range)
        {
            t.text = "<color=red>" + melee + "</color> <size=15> (" + range + ")</size>";
        }
        if (melee > range)
        {
            t.text = "<color=red>" + melee + "</color> <size=15> (" + range + ")</size>";
        }
        if (melee == range)
        {
            SetValuesAndColours(bonusRange, baseRange, t);
        }
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
            t.text += "<size=15> - </size><color=red>" + -bonusValue + "</color>";
        }
    }

    void OnBuffCreatedOrDestroyed(AbstractBuff buff)
    {
        Unit owner = buff.buffGroup.owner as Unit;
        if (owner != null && owner == myUnit)
        {
            AdjustTextValuesFor(myUnit);
        }
    }
}
