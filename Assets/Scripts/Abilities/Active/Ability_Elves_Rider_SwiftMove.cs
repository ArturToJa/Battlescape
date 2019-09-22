using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Ability_Elves_Rider_SwiftMove : Ability_Basic
{
    UnitMovement myUnitMovement;

    protected override void OnStart()
    {
        myUnitMovement = myUnit.GetComponent<UnitMovement>();
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
        myUnitMovement.CanMove = true;
    }

    protected override void CancelUse()
    {
        IsForcingMovementStuff = false;
        //myUnit.isQuittingCombat = false;
        myUnitMovement.CanMove = false;
    }




    protected override bool ActivationRequirements()
    {
        return (
           MouseManager.Instance.mouseoveredTile != null &&
           MovementQuestions.Instance.CanMove(MouseManager.Instance.SelectedUnit, MouseManager.Instance.mouseoveredTile, false)&&
           /*Pathfinder.Instance.GetAllTilesThatWouldBeLegalIfNotInCombat(myUnit, myUnitMovement.GetCurrentMoveSpeed(true)).Contains(MouseManager.Instance.mouseoveredTile)&&*/
           EventSystem.current.IsPointerOverGameObject() == false
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
        MovementSystem.Instance.DoMovement(myUnitMovement);
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
        Pathfinder.Instance.ColourPossibleTiles(myUnitMovement, true);
    }


    protected override void SetTarget()
    {
        Target = MouseManager.Instance.mouseoveredTile;
    }

}
