using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability_Neutral_SwapWeapons : Ability_Basic
{
    //CanBeSwapped is added for Hunters. If they have no more arrows, they can no longer swap.
    [HideInInspector] public bool CanBeSwapped = true;
    bool IsPri = true;
    public Weapon PrimaryWeapon;
    public Weapon SecondaryWeapon;
    [SerializeField] Sprite PrimarySprite;
    [SerializeField] Sprite SecondarySprite;


    protected override void OnStart()
    {
        PrimaryWeapon.Owner = myUnit;
        SecondaryWeapon.Owner = myUnit;
        Target = myUnit.currentPosition;
        PrimaryWeapon.ItemHidden.SetActive(false);
        SecondaryWeapon.ItemInUse.SetActive(false);
        PrimaryWeapon.SetLogicallyInUse(true);
    }

    protected override void OnUpdate()
    {
        if (myUnit == MouseManager.Instance.SelectedUnit)
        {
            if (IsPri)
            {
                MyObject.GetComponentInChildren<AbilityIconScript>().myImage.sprite = SecondarySprite;
            }
            else
            {
                MyObject.GetComponentInChildren<AbilityIconScript>().myImage.sprite = PrimarySprite;
            }
        }
    }

    protected override bool IsUsableNow()
    {
        return (CanBeSwapped == true);
    }

    protected override void CancelUse()
    {
        return;
    }

    protected override void Use()
    {
        SendCommandForActivation();
    }

    protected override bool ActivationRequirements()
    {
        return true;
    }

    public override void Activate()
    {
        DoSwap();
    }

    public void DoSwap()
    {
        if (IsPri)
        {
            StartCoroutine(Swap(SecondaryWeapon, PrimaryWeapon));
        }
        else
        {
            StartCoroutine(Swap(PrimaryWeapon, SecondaryWeapon));
        }
        IsPri = !IsPri;
    }



    IEnumerator Swap(Weapon swapIn, Weapon swapOut)
    {
        //Setting unit as a Ranged (or not) unit, both logically and visually
        myUnit.attackType = swapIn.attackType;
        GetComponentInChildren<AnimController>().SetRanged(swapIn.attackType == BattlescapeLogic.AttackTypes.Ranged);

        //Adding (or deducting) stats based on stats of swapped weapons
        swapIn.SetLogicallyInUse(true);
        swapOut.SetLogicallyInUse(false);
        yield return null;
        FinishUsing();
    }  

    public override void AI_Activate(GameObject Target)
    {
        return;
    }

    public override GameObject AI_ChooseTarget()
    {
        return null;
    }

    public override bool AI_IsGoodToUseNow()
    {
        return false;
    }

    protected override void SetTarget()
    {
        // Due to specifics of this ability, this function does NOT ever run apparently, therefore we just set target on Start ;).
        return;
    }
}
