using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIStatsValues : MonoBehaviour
{
    [SerializeField] bool RealOne;
    [SerializeField] int Attack;
    [SerializeField] int Defence;
    [SerializeField] int Movement;
    [SerializeField] int Health;
    [SerializeField] int ShootingRange;
    [SerializeField] int Value;
    [SerializeField] int PoisonCounters;
    UnitScript unit;

    [SerializeField] Text attackText;
    [SerializeField] Text defenceText;
    [SerializeField] Text movementText;
    [SerializeField] Text healthText;
    [SerializeField] Text shootingRangeText;
    [SerializeField] Text valueText;
    [SerializeField] Text poisonCountersText;

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



        Attack = unit.CurrentAttack;
        Defence = unit.CurrentDefence;
        Health = unit.CurrentHP;
        PoisonCounters = unit.PoisonCounter;
        SetColourDependingOnSize(Attack, unit.GetBaseAttack(), attackText);
        SetColourDependingOnSize(Defence, unit.GetBaseDefence(), defenceText);
        SetColourDependingOnSize(Movement, unit.GetBaseMS(), movementText);
        SetColourDependingOnSize(Health, unit.MaxHP, healthText);
        if (unit.GetComponent<ShootingScript>() != null)
        {
            ShootingRange = unit.GetComponent<ShootingScript>().currShootingRange;
        }
        else
            ShootingRange = 0;
        
        Value = unit.Value;
        Movement = unit.GetComponent<UnitMovement>().GetCurrentMoveSpeed(false);
        return true;

    }

    void AdjustTextValues()
    {
        attackText.text = Attack.ToString();
        defenceText.text = Defence.ToString();
        movementText.text = Movement.ToString();
        healthText.text = Health.ToString();
        if (ShootingRange > 0)
        {
            shootingRangeText.transform.parent.parent.gameObject.SetActive(true);
            shootingRangeText.text = ShootingRange.ToString();
        }
        else
        {
            shootingRangeText.transform.parent.parent.gameObject.SetActive(false);
        }
        if (Value > 0)
        {
            valueText.transform.parent.parent.gameObject.SetActive(true);
            valueText.text = Value.ToString();
        }
        else
        {
            valueText.transform.parent.parent.gameObject.SetActive(false);
        }
        
        if (PoisonCounters > 0)
        {
            poisonCountersText.transform.parent.parent.gameObject.SetActive(true);
            poisonCountersText.text = PoisonCounters.ToString();
        }
        else
        {
            poisonCountersText.transform.parent.parent.gameObject.SetActive(false);
        }
    }

    void ChangeTextColourTo(Text text, Color colour)
    {
        text.color = colour;
    }
    void SetColourDependingOnSize(int currValue, int baseValue, Text t)
    {
        if (currValue < baseValue)
        {
            ChangeTextColourTo(t, Color.red);
        }
        if (currValue == baseValue)
        {
            ChangeTextColourTo(t, Color.white);
        }
        if (currValue > baseValue)
        {
            ChangeTextColourTo(t, Color.green);
        }
    }
}
