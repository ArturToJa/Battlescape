using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BattlescapeLogic;

public class UIHitChanceInformation : MonoBehaviour
{
    Text theText;

    void Awake()
    {
        theText = GetComponentInChildren<Text>();
        theText.text = "";
    } 
    
    public void TurnOnFor(Unit attacker, Unit defender)
    {
        UIManager.InstantlyTransitionActivity(gameObject, true);
        ShowNewInformation(attacker, defender);
    }
    public void TurnOff()
    {
        UIManager.InstantlyTransitionActivity(gameObject, false);
    }
    
    public void ShowNewInformation(Unit attacker, Unit defender)
    {
        float hitChance = DamageCalculator.HitChance(attacker, defender);
        theText.text = "Chances for:";
        theText.text += "\n" + "Miss (reducing Defence): " + ((1-hitChance) * 100).ToString() + "%";
        theText.text += "\n" + "Hit (dealing Damage): " + (hitChance * 100).ToString() + "%";
        int avgDmg = Statistics.baseDamage + DamageCalculator.GetStatisticsDifference(attacker, defender);
        int dmgRange = avgDmg / 5;
        theText.text += "\n" + "\n" + "Damage if hit: " + (avgDmg - dmgRange).ToString() + " - " + (avgDmg + dmgRange).ToString();
    }


   
}
