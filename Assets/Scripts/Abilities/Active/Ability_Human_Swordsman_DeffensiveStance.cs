using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability_Human_Swordsman_DeffensiveStance : Ability_Basic
{
    [SerializeField] GameObject vfx;

    protected override void OnStart()
    {
        Target = myUnit.currentPosition;
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






    public override bool ActivationRequirements()
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
        myUnit.statistics.movementPoints = 0;
        myUnit.statistics.numberOfAttacks = 0;
        PlayAbilitySound();
        CreateVFXOn(transform, BasicVFX.transform.rotation);
        //myUnit.GetComponent<AnimController>().Cast();        
        PassiveAbility_Buff.AddBuff(gameObject, 2, 0, 2, 0, myUnit.statistics.currentMaxNumberOfRetaliations + 1, "SwordmanBuff", vfx, 3f, false, false, true);
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
