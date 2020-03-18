using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using BattlescapeLogic;

public class Ability_Human_BK_Charge : Ability_Basic
{
    //public static bool IsForcingMovementStuff = false;
    [SerializeField] GameObject BonusMarker;
    [SerializeField] int AttackBuff;
    [SerializeField] int MSBuffValue;
    List<GameObject> BonusMarkers;
    bool alreadyAddedMarkers;

    protected override void OnStart()
    {
        BonusMarkers = new List<GameObject>();
    }

    protected override void OnUpdate()
    {
        return;
    }

    protected override bool IsUsableNow()
    {
        return true;
    }

    protected override void CancelUse()
    {
        //myUnit.statistics.maxMovementPoints -=MSBuffValue;
        DestroyMarkers();
    }

    protected override void Use()
    {
        //myUnit.statistics.maxMovementPoints -=MSBuffValue;
        IsForcingMovementStuff = true;
    }
    protected override void SetTarget()
    {
        Target = null; //MouseManager.Instance.mouseoveredTile;
    }

    public override void Activate()
    {
        Log.SpawnLog(myUnit.name + " uses Charge!");
        int tileCount = Pathfinder.instance.GetDistanceFromTo(myUnit, Target);
        StartCoroutine(Charge(tileCount));
    }

    public override bool ActivationRequirements()
    {
        return (true
            //MouseManager.Instance.mouseoveredTile != null &&
            //MovementQuestions.Instance.CanMove(GameManager.instance.selectedUnit, MouseManager.Instance.mouseoveredTile) &&
            //EventSystem.current.IsPointerOverGameObject() == false
            );
    }

    protected override void ColourTiles()
    {
        //Pathfinder.instance.ColourPossibleTiles(GetComponent<BattlescapeLogic.Unit>(), true);
        if (alreadyAddedMarkers == false)
        {
            alreadyAddedMarkers = true;
            foreach (Tile tile in Global.instance.currentMap.board)
            {

                if ((Pathfinder.instance.GetDistanceFromTo(myUnit, tile) == myUnit.statistics.movementPoints) && tile.IsProtectedByEnemyOf(myUnit) && tile.IsWalkable() && tile.hasObstacle == false)
                {
                    var temp = Instantiate(BonusMarker, tile.transform.position, Quaternion.identity, tile.transform);
                    BonusMarkers.Add(temp);
                }
            }
        }

    }

    IEnumerator Charge(int tileCount)
    {
        yield return null;
        FinishUsing();
        DestroyMarkers();
        CreateVFXOn(transform, transform.rotation);
        PlayAbilitySound();
        IsForcingMovementStuff = false;
        //PathCreator.Instance.AddSteps(myUnit, Target);
        //MovementSystem.Instance.SendCommandToMove(myUnit, Target);
        //^ no idea, if this should call SendCommandToMove or just DoMovement!
        while (myUnit.movement.isMoving)
        {
            yield return null;
        }
        CheckForBonus(tileCount);
        CheckForAttack(tileCount);
    }

    void CheckForBonus(int tileCount)
    {
        if (tileCount == myUnit.statistics.movementPoints && myUnit.IsInCombat())
        {
            PassiveAbility_Buff.AddBuff(myUnit.gameObject, 1, AttackBuff, 0, 0, myUnit.statistics.currentMaxNumberOfRetaliations, "BKBuff", null, 0, false, false, false);
            Log.SpawnLog(myUnit.name + " gets charging bonus!");
        }
    }

    void CheckForAttack(int tileCount)
    {
        if ((float)tileCount >= myUnit.statistics.movementPoints * 0.5f && myUnit.IsInCombat())
        {
            Attack();
        }
    }

    void Attack()
    {
        ////int RandomEnemy = Random.Range(0, myUnit.EnemyList.Count);
        ////CombatController.Instance.attackTarget = myUnit.EnemyList[RandomEnemy];
        //Log.SpawnLog(myUnit.name + " attack(s) " + CombatController.Instance.attackTarget.name + " while charging!");
        //// Notice that all of this is networked so we dont want to send the attack over network from BOTH computers, so ill send it just from the computer on which the player is LOCAL 
        //// (i dont even think i care which one computer does it tho tbh). But this fraze will work in Single/HS too.
        //if (Global.instance.IsCurrentPlayerLocal() || Global.instance.IsCurrentPlayerAI())
        //{
        //    CombatController.Instance.SendCommandToAttack(myUnit, CombatController.Instance.attackTarget, false);
        //}

        //if (Global.instance.IsCurrentPlayerAI())
        //{
        //    CombatController.Instance.MakeAIWait(3f);
        //}
    }

    void DestroyMarkers()
    {
        alreadyAddedMarkers = false;
        foreach (var item in BonusMarkers)
        {
            if (Application.isEditor)
            {
                DestroyImmediate(item);
            }
            else
            {
                Destroy(item);
            }
        }
        IsForcingMovementStuff = false;
    }


    ///////////////// AI segment

    public override bool AI_IsGoodToUseNow()
    {
        if (GameRound.instance.gameRoundCount >= 15)
        {
            return true;
        }
        if (GameRound.instance.gameRoundCount >= 9 || myUnit.statistics.healthPoints == 1)
        {
            int speed = myUnit.statistics.movementPoints;
            foreach (BattlescapeLogic.Unit enemy in VictoryLossChecker.GetEnemyUnitList())
            {
                int distance = Pathfinder.instance.GetDistanceFromTo(myUnit, enemy.currentPosition);
                if ((enemy.IsRanged() || enemy is Hero) && distance != -1 && (distance > speed && speed < distance + 2) || speed + 2 > distance && (float)(speed + 2) / 2 < distance)
                {
                    return true;
                }
            }
        }

        return false;
    }

    public override void AI_Activate(GameObject Target)
    {


        if (myUnit.currentPosition != Target)
        {
            Log.SpawnLog(myUnit.name + " uses Charge!");
            //PathCreator.Instance.AddSteps(myUnit, Target.GetComponent<Tile>());
            //StartCoroutine(Charge(PathCreator.Instance.Path.Count - 1));
        }
        else
        {
            UsesLeft--;
            // we just  "burn" the ability silently, without shpowing it to player - it is clearly the last turn of the game and we just really wanted to use it but could not.. Sad story!
        }
    }

    public override GameObject AI_ChooseTarget()
    {
        int speed = myUnit.statistics.movementPoints;
        foreach (BattlescapeLogic.Unit enemy in VictoryLossChecker.GetEnemyUnitList())
        {
            if (Pathfinder.instance.GetDistanceFromTo(myUnit, enemy.currentPosition) <= speed && (enemy.IsRanged() || enemy is Hero))
            {
                foreach (Tile neighbour in enemy.currentPosition.neighbours)
                {
                    //if (neighbour.IsWalkable() && Pathfinder.instance.WouldTileBeLegal(neighbour, myUnit, speed + 2))
                    //{
                    //    return neighbour.gameObject;
                    //}
                }
            }
        }
        foreach (BattlescapeLogic.Unit enemy in VictoryLossChecker.GetEnemyUnitList())
        {
            if (Pathfinder.instance.GetDistanceFromTo(myUnit, enemy.currentPosition) <= speed)
            {
                foreach (Tile neighbour in enemy.currentPosition.neighbours)
                {
                    //if (neighbour.IsWalkable() && Pathfinder.instance.WouldTileBeLegal(neighbour, myUnit, speed + 2))
                    //{
                    //    return neighbour.gameObject;
                    //}
                }
            }
        }
        // Here we have NO targets at all.. IMO we can move just by one for no reason at all xD
        foreach (Tile neighbour in myUnit.currentPosition.neighbours)
        {
            if (neighbour.IsWalkable())
            {
                Debug.Log(neighbour.name);
                return neighbour.gameObject;
            }
        }
        Debug.Log(myUnit.currentPosition.name);
        return myUnit.currentPosition.gameObject;
    }
}
