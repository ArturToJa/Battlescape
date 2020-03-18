using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BattlescapeLogic;

public class UIHitChanceInformation : MonoBehaviour
{
    Text theText;

    public static UIHitChanceInformation instance { get; private set; }    

    void Awake()
    {
        theText = GetComponentInChildren<Text>();
        theText.text = "";
        if (instance != null)
        {
            Debug.LogError("Doubled HitChanceInfo, there can be only one!");
            return;
        }
        instance = this;
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
        theText.text += "\n" + "Miss (reducing Defence): " + ((1-hitChance) * 100).ToString("F2") + "%";
        theText.text += "\n" + "Hit (dealing Damage): " + (hitChance * 100).ToString("F2") + "%";
        int avgDmg = Statistics.baseDamage + DamageCalculator.GetStatisticsDifference(attacker, defender);
        int dmgRange = avgDmg / 5;
        theText.text += "\n" + "\n" + "Damage if hit: " + (avgDmg - dmgRange).ToString() + " - " + (avgDmg + dmgRange).ToString();
    }

    public void OnMouseHoverEnter(Unit hoveredUnit)
    {
        if (GameRound.instance.currentPlayer.selectedUnit != null && GameRound.instance.currentPlayer.selectedUnit.attack.CanAttack(hoveredUnit))
        {
            TurnOnFor(GameRound.instance.currentPlayer.selectedUnit, hoveredUnit);
        }
    }


   
}
