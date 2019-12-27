using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability_Hero_Knight_Battlecry : Ability_Basic
{
    [SerializeField] int Range;
    [SerializeField] GameObject RingOfBuff;
    [SerializeField] GameObject VFXOnBuffedAllies;
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
    }






    public override bool ActivationRequirements()
    {
        return true;
    }

    public override void Activate()
    {
        StartCoroutine(Battlecry());
    }


    IEnumerator Battlecry()
    {
        //myUnit.GetComponent<AnimController>().Cast();
        Log.SpawnLog(myUnit.name + " uses Battlecry, increasing nearby allies' attack and removing all negative effects.");


        //foreach (BattlescapeLogic.Unit ally in Helper.GetAlliesInRange(myUnit, Range))
        //{
        //    StartCoroutine(PassiveAbility_Buff_BattlecryBuff.AddMyBuff(myUnit, ally, VFXOnBuffedAllies, RingOfBuff, 1.5f));
        //}

        yield return null;
        FinishUsing();
    }





    protected override void SetTarget()
    {
        return;
    }
    public override void OnHover()
    {
        //TileBattlescapeGraphics.ColouringTool.UncolourAllTiles();
        ColourTiles();
    }

    protected override void ColourTiles()
    {
        //foreach (BattlescapeLogic.Unit ally in Helper.GetAlliesInRange(myUnit, Range))
        //{
        //    BattlescapeGraphics.ColouringTool.ColourObject(ally.currentPosition, Color.green);
        //}
    }





    public override void AI_Activate(GameObject Target)
    {
        SendCommandForActivation();
    }

    public override GameObject AI_ChooseTarget()
    {
        return myUnit.currentPosition.gameObject;
    }

    public override bool AI_IsGoodToUseNow()
    {
        //return Helper.GetAlliesInRange(myUnit, Range).Count > 2 || TurnManager.Instance.TurnCount >= 15;
        return false;
    }



}
