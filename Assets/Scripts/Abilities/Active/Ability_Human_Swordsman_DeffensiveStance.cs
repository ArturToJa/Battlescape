using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability_Human_Swordsman_DeffensiveStance : Ability_Basic
{

    UnitMovement myMovement;
    [SerializeField] GameObject vfx;

    protected override void OnStart()
    {
        myMovement = myUnit.GetComponent<UnitMovement>();
        Target = myUnit.myTile;
    }

    protected override void OnUpdate()
    {
        return;
    }





    protected override bool IsUsableNow()
    {
        return
            true;
    }

    protected override void Use()
    {
        SendCommandForActivation();
    }

    protected override void CancelUse()
    {
        return;
        //impossible
    }






    protected override bool ActivationRequirements()
    {
        return true;
    }

    public override void Activate()
    {
        StartCoroutine(DefensiveStance());
    }

    IEnumerator DefensiveStance()
    {
        yield return null;
        FinishUsing();
        myMovement.CanMove = false;
        myUnit.hasAttacked = true;
        PlayAbilitySound();
        CreateVFXOn(transform, BasicVFX.transform.rotation);
        GetComponent<AnimController>().Cast();        
        PassiveAbility_Buff.AddBuff(gameObject, 2, 0, 2, 0, 0, true, "SwordmanBuff", vfx, 3f, false, false, true);
    }









    public override void AI_Activate(GameObject Target)
    {
        SendCommandForActivation();
    }

    public override GameObject AI_ChooseTarget()
    {
        return null;
    }

    public override bool AI_IsGoodToUseNow()
    {
        //idk how to make AI use it
        return false;
    }

   

   

    

   
    protected override void SetTarget()
    {
        return;
    }

    
}
