using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementSoundTool : MonoBehaviour
{

    UnitScript myUnit;
    ShootingScript myShooting;
    Ability_Elves_Wolf_Howl myHowler;
    Ability_Neutral_SwapWeapons mySwapper;
    Ability_Elves_Assassin_DaggerThrow myThrower;
    Ability_Hero_Knight_Battlecry myCry;
    Ability_Human_Pikeman_SpearBarricade myPikeman;
    GameObject PikeVisible;
    [SerializeField] bool IsReal = true;

    private void Start()
    {
        myUnit = this.transform.root.GetComponent<UnitScript>();
        myShooting = this.transform.root.GetComponent<ShootingScript>();
        myHowler = this.transform.root.GetComponent<Ability_Elves_Wolf_Howl>();
        mySwapper = this.transform.root.GetComponent<Ability_Neutral_SwapWeapons>();
        myThrower = this.transform.root.GetComponent<Ability_Elves_Assassin_DaggerThrow>();
        myCry = this.transform.root.GetComponent<Ability_Hero_Knight_Battlecry>();
        myPikeman = this.transform.root.GetComponent<Ability_Human_Pikeman_SpearBarricade>();
    }

    void Step()
    {
        this.transform.root.GetComponent<UnitMovement>().Step();
    }

    void Hit()
    {
        myUnit.Hit();
    }
    void Swing()
    {
        myUnit.Swing();
    }
    void Shoot()
    {
        myShooting.Shoot();
    }
    void HOAShoot()
    {
        myShooting.PlayRandomShot();
    }

    void Death()
    {
        myUnit.DeathSound();
    }
    void Howl()
    {
        myHowler.Howl();
    }
    void Throw()
    {
        myThrower.LaunchDagger(myThrower.Target.transform.position, myThrower.Speed);
    }
    void Casted()
    {
        if (myCry != null)
        {
            myCry.CreateVFXOn(myCry.transform, myCry.BasicVFX.transform.rotation);
            myCry.PlayAbilitySound();
        }
    }

    void SpearToss()
    {
        if (myPikeman != null)
        {
            PikeVisible = Helper.FindChildWithTag(gameObject, "SpearInHand");
            PikeVisible.SetActive(false);
            if (IsReal)
            {
                myPikeman.PlayAbilitySound();
            }

        }
        else
        {
            Debug.LogError("WTF");
        }
    }

    void UnequipPri()
    {
        mySwapper.PrimaryWeapon.SetVisuallyInUse(false);
    }
    void UnequipSec()
    {
        mySwapper.SecondaryWeapon.SetVisuallyInUse(false);
    }
    void EquipPri()
    {
        mySwapper.PrimaryWeapon.SetVisuallyInUse(true);
    }
    void EquipSec()
    {
        if (mySwapper != null)
        {
            mySwapper.SecondaryWeapon.SetVisuallyInUse(true);
        }
        else
        {
            PikeVisible.SetActive(true);
            Helper.FindChildWithTag(gameObject, "HiddenSpear").SetActive(false);
        }
    }
}
