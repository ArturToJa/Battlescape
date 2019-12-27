using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using BattlescapeLogic;

public class Ability_Elves_Rider_SwiftMove : Ability_Basic
{
    int previousMovementPoints;
    protected override void OnStart()
    {
        return;
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
        IsForcingMovementStuff = true;
        // myUnit.isQuittingCombat = true;
        previousMovementPoints = myUnit.statistics.movementPoints;
        myUnit.statistics.movementPoints = myUnit.statistics.GetCurrentMaxMovementPoints();
    }

    protected override void CancelUse()
    {
        IsForcingMovementStuff = false;
        //myUnit.isQuittingCombat = false;
        myUnit.statistics.movementPoints = previousMovementPoints;
    }




    public override bool ActivationRequirements()
    {
        return (
           //MouseManager.Instance.mouseoveredTile != null &&
           //MovementQuestions.Instance.CanMove(GameManager.instance.selectedUnit, MouseManager.Instance.mouseoveredTile) &&
           ///*Pathfinder.instance.GetAllTilesThatWouldBeLegalIfNotInCombat(myUnit, myBattlescapeLogic.Unit.GetCurrentMoveSpeed(true)).Contains(MouseManager.Instance.mouseoveredTile)&&*/
           //EventSystem.current.IsPointerOverGameObject() == false
           true
           );
    }

    public override void Activate()
    {
        StartCoroutine(SwiftMove());
    }

    IEnumerator SwiftMove()
    {
        Log.SpawnLog("Rider swiftly gallops away from combat!");
        IsForcingMovementStuff = false;
        //myUnit.isQuittingCombat = false;
        PlayAbilitySound();
        CreateVFXOn(transform, BasicVFX.transform.rotation);
        myUnit.Move(Target);
        yield return null;
        FinishUsing();
    }





    public override void AI_Activate(GameObject Target)
    {
        Debug.LogError("Why is AI using this ability?");
    }

    public override GameObject AI_ChooseTarget()
    {
        // i dont care as i cannot make good algorithm for AI to make calls with this ability
        return null;
    }

    public override bool AI_IsGoodToUseNow()
    {
        return false;
        // right now we dont want AI to use this, cause i cannot into AI
    }

    protected override void ColourTiles()
    {
        BattlescapeGraphics.ColouringTool.ColourLegalTilesFor(myUnit);
    }


    protected override void SetTarget()
    {
        Target = null; //MouseManager.Instance.mouseoveredTile;
    }

}
