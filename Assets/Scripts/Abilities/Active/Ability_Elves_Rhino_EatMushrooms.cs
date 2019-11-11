using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BattlescapeLogic;

public class Ability_Elves_Rhino_EatMushrooms : Ability_Basic
{
    [SerializeField] int healValue;

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
       List<Tile> legalTiles = Pathfinder.instance.GetAllLegalTilesFor(myUnit);
        foreach (var tile in legalTiles)
        {
            if (HasTileFood(tile))
            {
                BattlescapeGraphics.ColouringTool.SetColour(tile, Color.cyan);
            }
        }
    }





    protected override bool ActivationRequirements()
    {
        return
            MouseManager.Instance.mouseoveredTile != null &&
            Pathfinder.instance.IsLegalTileForUnit(MouseManager.Instance.mouseoveredTile, myUnit) &&
            HasTileFood(MouseManager.Instance.mouseoveredTile);
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
        MovementSystem.Instance.DoMovement(myUnit,Target);
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
        return myUnit.movement.isMoving == false;
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

    bool HasTileFood(Tile tile)
    {
        return tile.GetComponentInChildren<RhinoFood>() != null;
    }

}
