using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BattlescapeLogic;

public class HeroRaceVisualChooser : MonoBehaviour
{

    // Use this for initialization
    void Awake()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
        int ID = 0;
        if (GetComponentInParent<UnitScript>().isPlayerOne == false)
        {
            ID = 1;
        }
        switch (Global.instance.playerTeams[ID].Players[0].race)
        {
            case Faction.Human:
                GetComponentInParent<AnimController>().MyAnimator = transform.GetChild(0).GetComponent<Animator>();
                GetComponentInParent<UnitMovement>().myBody = transform.GetChild(0);                
                transform.GetChild(0).gameObject.SetActive(true);
                break;
            case Faction.Elves:
                GetComponentInParent<AnimController>().MyAnimator = transform.GetChild(1).GetComponent<Animator>();
                GetComponentInParent<UnitMovement>().myBody = transform.GetChild(1);
                transform.GetChild(1).gameObject.SetActive(true);
                break;
            case Faction.Neutral:
                Debug.LogError("This hero is neutral and should probably not be: " + transform.root.position);
                break;
            default:
                break;
        }
        HeroScript hs = GetComponentInParent<HeroScript>();
        if (hs == null)
        {
            return;
        }
        Transform[] children = GetComponentsInChildren<Transform>();
        foreach (Transform child in children)
        {
            if (child.gameObject.activeSelf && child.gameObject.tag == "HiddenSecWeapon")
            {
                hs.secondaryWeaponHidden = child.gameObject;
                child.gameObject.SetActive(!hs.secondaryWeaponInHandOnStart);
            }
            if (child.gameObject.activeSelf && child.gameObject.tag == "SecWeapon")
            {
                hs.secondaryWeaponInHand = child.gameObject;
                child.gameObject.SetActive(hs.secondaryWeaponInHandOnStart);
            }
            if (child.gameObject.activeSelf && child.gameObject.tag == "HiddenPriWeapon")
            {
                hs.primaryWeaponHidden = child.gameObject;
            }
            if (child.gameObject.activeSelf && child.gameObject.tag == "PriWeapon")
            {
                hs.primaryWeaponInHand = child.gameObject;
            }
        }
    }   
}
