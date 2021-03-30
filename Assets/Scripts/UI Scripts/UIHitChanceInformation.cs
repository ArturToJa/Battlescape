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
    
    public void TurnOnFor(IDamageSource potentialDamageSource, IDamageable target)
    {
        UIManager.InstantlyTransitionActivity(gameObject, true);
        ShowNewInformation(potentialDamageSource, target);
    }
    public void TurnOff()
    {
        UIManager.InstantlyTransitionActivity(gameObject, false);
    }
    
    void ShowNewInformation(IDamageSource aggressor, IDamageable target)
    {
        PotentialDamage potentialDamage = aggressor.GetPotentialDamageAgainst(target);
        theText.text = potentialDamage.GetText();       
    }        

    public void OnMouseHoverEnter(IDamageable hoveredObject)
    {
        if (CouldDamageNow(hoveredObject))
        {
            TurnOnFor((GetCurrentDamagingEntity()), hoveredObject);
        }
    }

    bool CouldDamageNow(IDamageable hoveredObject)
    {
        IDamageSource source = GetCurrentDamagingEntity();
        return source != null && source.CanPotentiallyDamage(hoveredObject) == true;
    }

    IDamageSource GetCurrentDamagingEntity()
    {
        IDamageSource sourceOfDamage = null;
        if (Global.instance.currentEntity is IDamageSource)
        {
            sourceOfDamage = Global.instance.currentEntity as IDamageSource;
        }
        else if (Global.instance.currentEntity is Unit)
        {
            sourceOfDamage = (Global.instance.currentEntity as Unit).attack;           
        }
        return sourceOfDamage;
    }


}
