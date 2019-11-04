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
        int raceNumber = (int)Global.instance.playerTeams[ID].players[0].race;
        UnitScript myUnit = GetComponentInParent<UnitScript>();
        GetComponentInParent<AnimController>().MyAnimator = transform.GetChild(raceNumber).GetComponent<Animator>();
        myUnit.visuals = transform.GetChild(raceNumber).gameObject;
        transform.GetChild(raceNumber).gameObject.SetActive(true);
        myUnit.animator = myUnit.visuals.GetComponent<Animator>();
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
