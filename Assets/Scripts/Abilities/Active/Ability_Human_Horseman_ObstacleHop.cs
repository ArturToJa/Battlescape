using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BattlescapeLogic;

public class Ability_Human_Horseman_ObstacleHop : Ability_Basic
{
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
        return;
    }

    protected override void CancelUse()
    {
        return;
    }










    protected override bool ActivationRequirements()
    {
        return
            MouseManager.Instance.mouseoveredTile != null &&
            MouseManager.Instance.mouseoveredTile.IsWalkable() &&
            Helper.AreTilesInRange(MouseManager.Instance.mouseoveredTile, myUnit.myTile, 2) &&
            IsValidJump(myUnit.myTile, MouseManager.Instance.mouseoveredTile);
        //there needs to be one more thing - the obstacle! in exact middle!
    }

    bool IsValidJump(Tile start, Tile end)
    {
        int startX = Mathf.RoundToInt(start.transform.position.x);
        int startZ = Mathf.RoundToInt(start.transform.position.z);
        int endX = Mathf.RoundToInt(end.transform.position.x);
        int endZ = Mathf.RoundToInt(end.transform.position.z);
        if (
            startX != endX &&
            startZ != endZ &&
                (
                Mathf.Abs(startX - endX) == 1 ||
                Mathf.Abs(startZ - endZ) == 1
                )
            )
        {
            return false;
        }
        int middleX = (startX + endX) / 2;
        int middleZ = (startZ + endZ) / 2;

        Tile midTile = Map.Board[middleX, middleZ];
        return
            midTile.hasObstacle &&
            midTile.GetComponentInChildren<Obstacle_Cover>() != null &&
            midTile.GetComponentInChildren<Obstacle_Cover>().IsJumpable;
    }

    public override void Activate()
    {
        StartCoroutine(Hop());
    }

    IEnumerator Hop()
    {
        Log.SpawnLog("Horseman jumps above an obstacle!");
        PlayAbilitySound();
        GameObject lightnings = CreateVFXOn(transform, BasicVFX.transform.rotation);
        GetComponent<AnimController>().SetJumping(true);
        myUnit.Move(Target);
        yield return null;
        FinishUsing();
        // some future
        //yield return WaitUntil(something)
        yield return new WaitUntil(IsDoneMoving);
        GetComponent<AnimController>().SetJumping(false);
        Destroy(lightnings);
        Target.SetMyUnitTo(myUnit);
    }


    bool IsDoneMoving()
    {
        return myUnit.newMovement.isMoving == false;
    }

    public override void AI_Activate(GameObject Target)
    {
        SendCommandForActivation();
    }

    public override GameObject AI_ChooseTarget()
    {
        // ??
        return null;
    }

    public override bool AI_IsGoodToUseNow()
    {
        return false;
    }








    protected override void SetTarget()
    {
        Target = MouseManager.Instance.mouseoveredTile;
    }

    protected override void ColourTiles()
    {
        var Tiles = Helper.GetTilesInRangeOf(myUnit.myTile, 2);
        foreach (Tile tile in Tiles)
        {
            if (
                myUnit.myTile.neighbours.Contains(tile) == false &&
                IsValidJump(tile, myUnit.myTile) &&
                tile.IsWalkable()
                )
            {
                ColouringTool.SetColour(tile, Color.green);
            }
        }
    }


}
