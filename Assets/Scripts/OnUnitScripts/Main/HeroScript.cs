using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroScript : MonoBehaviour
{
    // TODO: Implement Weapon/Item system here!

    public GameObject primaryWeaponHidden;
    public GameObject primaryWeaponInHand;
    public GameObject secondaryWeaponInHand;
    public GameObject secondaryWeaponHidden;
    public bool secondaryWeaponInHandOnStart = false;

    // note that Shooting heroes have a sword as "secondary" weapons. Lets localize them:


    public void ToggleWeapon(GameObject weaponIn, GameObject weaponOut)
    {
        weaponIn.SetActive(true);
        weaponOut.SetActive(false);
    }


}
