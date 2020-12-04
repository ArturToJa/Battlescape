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
        
        if (instance != null)
        {
            Debug.LogError("Doubled HitChanceInfo, there can be only one!");
            return;
        }
        instance = this;
        theText = GetComponentInChildren<Text>();
        theText.text = "";
        TurnOff();
    } 
    
    public void TurnOnFor(Unit attackingUnit, IDamageable target)
    {
        UIManager.InstantlyTransitionActivity(gameObject, true);
        ShowNewInformation(attackingUnit, target);
    }
    public void TurnOff()
    {
        UIManager.InstantlyTransitionActivity(gameObject, false);
    }
    
    void ShowNewInformation(Unit attackingUnit, IDamageable target)
    {
        float hitChance = target.ChanceOfBeingHitBy(attackingUnit);
        theText.text = "Chances for:";
        theText.text += "\n" + "Miss (reducing Defence): " + ((1-hitChance) * 100).ToString("F2") + "%";
        theText.text += "\n" + "Hit (dealing Damage): " + (hitChance * 100).ToString("F2") + "%";
        if (hitChance > 0)
        {
            int avgDmg = DamageCalculator.GetAvarageDamage(attackingUnit, target);
            int dmgRange = avgDmg / 5;
            theText.text += "\n" + "\n" + "Damage if hit: " + (avgDmg - dmgRange).ToString() + " - " + (avgDmg + dmgRange).ToString();
        }
    }        

    public void OnMouseHoverEnter(IDamageable hoveredObject)
    {
        if (GameRound.instance.currentPlayer.selectedUnit != null && GameRound.instance.currentPlayer.selectedUnit.attack.CanAttack(hoveredObject))
        {
            TurnOnFor(GameRound.instance.currentPlayer.selectedUnit, hoveredObject);
        }
    }


   
}
