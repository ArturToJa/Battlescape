using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BattlescapeLogic;

public class Ability_Hero_Ranger_ShadowCover : Ability_Basic
{
    [SerializeField] int Range;
    List<Tile> legalTiles;

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
        return myUnit.statistics.movementPoints > 0;
    }

    protected override void CancelUse()
    {
        return;
    }

    protected override void Use()
    {
        return;
    }

    protected override void SetTarget()
    {
        Target = null; //MouseManager.Instance.mouseoveredTile;
    }

    public override void Activate()
    {
        StartCoroutine(ShadowCover(Target));
    }

    public override bool ActivationRequirements()
    {
        return true; //MouseManager.Instance.mouseoveredTile != null && legalTiles.Contains(MouseManager.Instance.mouseoveredTile);
    }

    protected override void ColourTiles()
    {
        legalTiles = new List<Tile>();
        Bounds RangeBounds = new Bounds(transform.position, new Vector3(2f * Range, 5f, 2f * Range));
        foreach (Tile tile in Map.Board)
        {
            if (RangeBounds.Contains(tile.transform.position) && tile.myUnit == null && tile.IsWalkable() && tile.hasObstacle == false && tile.IsProtectedByEnemyOf(myUnit) == false)
            {
                BattlescapeGraphics.ColouringTool.ColourObject(tile, Color.green);
                legalTiles.Add(tile);
            }
        }
    }

    IEnumerator ShadowCover(Tile tile)
    {
        Log.SpawnLog(myUnit.name + " uses Shadow Cover, leaving combat safely!");
        PlayAbilitySound();
        CreateVFXOn(transform, BasicVFX.transform.rotation);

        //myUnit.movement = Global.instance.movementTypes[(int)MovementTypes.Teleportation]
        //myUnit.movement.ApplyUnit(myUnit);
        //myUnit.movement.MoveTo(tile);
        myUnit.statistics.movementPoints = 0;
        yield return null;
        FinishUsing();
    }

    public override bool AI_IsGoodToUseNow()
    {
        return /*((myUnit.EnemyList.Count >= 2) || (myUnit.statistics.healthPoints == 1 && myUnit.EnemyList.Count > 0))*/false;
    }

    public override void AI_Activate(GameObject Target)
    {
        throw new System.NotImplementedException();
    }

    public override GameObject AI_ChooseTarget()
    {
        // I admit this is some HORRIBLE code down there. Maybe i should have just made this random, since my AI code is in general SICKly bad.. It should just generally work as follows in final version (as it does now):
        // Just take the normal movement AI and make it find the best move from legal tiles for this ability.

        AI_Movement TemporaryMovementAI = new AI_Movement(myUnit.owner.index);
        var dictionary = new Dictionary<Tile, float>();
        foreach (Tile tile in AI_GetTheLegalMoves())
        {
            dictionary.Add(tile, TemporaryMovementAI.EvaluateTile(myUnit, tile, TemporaryMovementAI.FindDirectionOfEnemies(myUnit), TemporaryMovementAI.GetAllEnemyMoves()));
        }
        return TemporaryMovementAI.GetTheMove(myUnit, dictionary).Key.gameObject;
    }

    List<Tile> AI_GetTheLegalMoves()
    {
        legalTiles = new List<Tile>();
        Bounds RangeBounds = new Bounds(transform.position, new Vector3(2f * Range, 5f, 2f * Range));
        foreach (Tile tile in Map.Board)
        {
            if (RangeBounds.Contains(tile.transform.position) && tile.myUnit == null && tile.IsWalkable() && tile.hasObstacle == false && tile.IsProtectedByEnemyOf(myUnit) == false)
            {
                legalTiles.Add(tile);
            }
        }
        return legalTiles;
    }
}
