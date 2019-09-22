using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class InfoUIOnQCChances : MonoBehaviour
{
    [SerializeField] Text thisText;

    void Update()
    {
        UnitScript currentUnit = MouseManager.Instance.SelectedUnit;
        if (currentUnit == null)
        {
            return;
        }
        thisText.text = "Enemies to escape: " + currentUnit.EnemyList.Count + "\n" + "Chances of getting attacked: " + CountTheChances(currentUnit);
    }

    private string CountTheChances(UnitScript currentUnit)
    {
         float result = Mathf.Pow(currentUnit.QuitCombatPercent * 0.01f, currentUnit.EnemyList.Count);
        if (result == 1)
        {
            return "None";
        }
        if (result >= 0.9f)
        {
            return "Marginal";
        }
        if (result >= 0.75f)
        {
            return "Small";
        }
        if (result >= 0.66f)
        {
            return "Medium";
        }
        if (result >= 0.5f)
        {
            return "Significant";
        }
        if (result >= 0.33f)
        {
            return "High";
        }
        if (result >= 0.25f)
        {
            return "Very High";
        }
        if (result >= 0.1f)
        {
            return "Extreme";
        }
        if (result > 0)
        {
            return "Nearly sure";
        }
        return "Sure";
       
    }
}
