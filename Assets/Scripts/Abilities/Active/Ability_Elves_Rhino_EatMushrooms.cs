using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability_Elves_Rhino_EatMushrooms : Ability_Basic
{
    [SerializeField] int healValue;
    UnitMovement myMovement;

    protected override void OnStart()
    {
        myMovement = myUnit.GetComponent<UnitMovement>();
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
        RhinoFood.SetAllActiveTo(true);
        IsForcingMovementStuff = true;
    }

    protected override void CancelUse()
    {
        RhinoFood.SetAllActiveTo(false);
        IsForcingMovementStuff = false;
    }

    protected override void ColourTiles()
    {
        Pathfinder.Instance.ColourPossibleTiles(myMovement, true);
    }





    protected override bool ActivationRequirements()
    {
        return
            MouseManager.Instance.mouseoveredTile != null &&
            Pathfinder.Instance.WouldTileBeLegal(MouseManager.Instance.mouseoveredTile, myUnit, myMovement.GetCurrentMoveSpeed(false)) &&
            MouseManager.Instance.mouseoveredTile.GetComponentInChildren<RhinoFood>() != null;
    }

    public override void Activate()
    {
        StartCoroutine(Eat());
    }

    IEnumerator Eat()
    {
        yield return null;
        FinishUsing();

        RhinoFood.SetAllActiveTo(false);
        Log.SpawnLog(myUnit.name + " eats a mushroom, gaining " + healValue + " health back");
        MovementSystem.Instance.DoMovement(myMovement);
        yield return new WaitUntil(EndedMovement);
        CreateVFXOn(transform, BasicVFX.transform.rotation);
        PlayAbilitySound();
        Destroy(Target.GetComponentInChildren<RhinoFood>().gameObject, 0.1f);        
        GetComponent<AnimController>().SetEating(true);
        myUnit.statistics.healthPoints += healValue;
        IsForcingMovementStuff = false;
        yield return new WaitForSeconds(3f);
        GetComponent<AnimController>().SetEating(false);
    }

    
   bool EndedMovement()
    {
        return
            myMovement.isMoving == false;
    }
    

   
   
    protected override void SetTarget()
    {
        Target = MouseManager.Instance.mouseoveredTile;
    }






    public override void AI_Activate(GameObject Target)
    {
        throw new System.NotImplementedException();
    }

    public override GameObject AI_ChooseTarget()
    {
        throw new System.NotImplementedException();
    }

    public override bool AI_IsGoodToUseNow()
    {
        throw new System.NotImplementedException();
    }

}
