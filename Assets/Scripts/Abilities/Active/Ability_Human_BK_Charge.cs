using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

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
        GetComponent<UnitMovement>().IncrimentMoveSpeedBy(-MSBuffValue);
        DestroyMarkers();
    }

    protected override void Use()
    {
        GetComponent<UnitMovement>().IncrimentMoveSpeedBy(MSBuffValue);
        IsForcingMovementStuff = true;
    }
    protected override void SetTarget()
    {
        Target = MouseManager.Instance.mouseoveredTile;
    }

    public override void Activate()
    {
        Log.SpawnLog(myUnit.name + " uses Charge!");
        Pathfinder.Instance.BFS(myUnit.myTile, true);
        int tileCount = Pathfinder.Instance.GetDistanceFromTo(myUnit.myTile, Target, true);
        StartCoroutine(Charge(tileCount));
    }

    protected override bool ActivationRequirements()
    {
        return (
            MouseManager.Instance.mouseoveredTile != null && 
            MovementQuestions.Instance.CanMove(MouseManager.Instance.SelectedUnit, MouseManager.Instance.mouseoveredTile, true) && 
            EventSystem.current.IsPointerOverGameObject() == false
            );
    }

    protected override void ColourTiles()
    {
        Pathfinder.Instance.ColourPossibleTiles(GetComponent<UnitMovement>(), true);
        if (alreadyAddedMarkers == false)
        {
            alreadyAddedMarkers = true;
            foreach (Tile tile in Map.Board)
            {
                
                if ((Pathfinder.Instance.GetDistanceFromTo(myUnit.myTile, tile, true) == GetComponent<UnitMovement>().GetCurrentMoveSpeed(true)) && tile.IsOccupiedByEnemy(myUnit.PlayerID) && tile.isWalkable && tile.hasObstacle == false)
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
        PathCreator.Instance.AddSteps(myUnit.myTile,Target);
        MovementSystem.Instance.SendCommandToMove(myUnit.GetComponent<UnitMovement>());
        while (myUnit.GetComponent<UnitMovement>().isMoving)
        {
            yield return null;
        }
        CheckForBonus(tileCount);
        CheckForAttack(tileCount);
    }

    void CheckForBonus(int tileCount)
    {
        if (tileCount == myUnit.GetComponent<UnitMovement>().GetCurrentMoveSpeed(true) && myUnit.CheckIfIsInCombat())
        {
            PassiveAbility_Buff.AddBuff(myUnit.gameObject, 1, AttackBuff, 0, 0, 0, true, "BKBuff", null, 0, false, false, false);
            Log.SpawnLog(myUnit.name + " gets charging bonus!");
        }
    }

    void CheckForAttack(int tileCount)
    {
        if ((float)tileCount >= myUnit.GetComponent<UnitMovement>().GetCurrentMoveSpeed(true) * 0.5f && myUnit.CheckIfIsInCombat())
        {
            Attack();
        }
    }

    void Attack()
    {
        int RandomEnemy = Random.Range(0, myUnit.EnemyList.Count);
        CombatController.Instance.AttackTarget = myUnit.EnemyList[RandomEnemy];
        Log.SpawnLog(myUnit.name + " attack(s) " + CombatController.Instance.AttackTarget.name + " while charging!");
        // Notice that all of this is networked so we dont want to send the attack over network from BOTH computers, so ill send it just from the computer on which the player is LOCAL 
        // (i dont even think i care which one computer does it tho tbh). But this fraze will work in Single/HS too.
        if (GameStateManager.Instance.IsCurrentPlayerLocal() || GameStateManager.Instance.IsCurrentPlayerAI())
        {
            CombatController.Instance.SendCommandToAttack(myUnit, CombatController.Instance.AttackTarget, false, false);
        }

        if (GameStateManager.Instance.IsCurrentPlayerAI())
        {
            CombatController.Instance.MakeAIWait(3f);
        }
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
        if (TurnManager.Instance.TurnCount >= 15)
        {
            return true;
        }
        if (TurnManager.Instance.TurnCount >= 9 || myUnit.CurrentHP == 1)
        {
            int speed = myUnit.GetComponent<UnitMovement>().GetCurrentMoveSpeed(true);
            foreach (UnitScript enemy in VictoryLossChecker.GetEnemyUnitList())
            {
                int distance = Pathfinder.Instance.GetDistanceFromTo(myUnit.myTile, enemy.myTile, true);
                if ((enemy.isRanged || enemy.GetComponent<HeroScript>() != null) && distance != -1 && (distance > speed && speed < distance + 2) || speed + 2 > distance && (float)(speed + 2) / 2 < distance)
                {
                    return true;
                }
            }
        }
        
        return false;
    }

    public override void AI_Activate(GameObject Target)
    {
        
        
        if (myUnit.myTile != Target)
        {
            Log.SpawnLog(myUnit.name + " uses Charge!");
            PathCreator.Instance.AddSteps(myUnit.myTile,Target.GetComponent<Tile>());
            StartCoroutine(Charge(PathCreator.Instance.Path.Count - 1));
        }
        else
        {
            UsesLeft--;
            // we just  "burn" the ability silently, without shpowing it to player - it is clearly the last turn of the game and we just really wanted to use it but could not.. Sad story!
        }
    }

    public override GameObject AI_ChooseTarget()
    {
        int speed = myUnit.GetComponent<UnitMovement>().GetCurrentMoveSpeed(true);
        foreach (UnitScript enemy in VictoryLossChecker.GetEnemyUnitList())
        {
            if (Pathfinder.Instance.GetDistanceFromTo(myUnit.myTile, enemy.myTile, true) <= speed &&(enemy.isRanged || enemy.GetComponent<HeroScript>() != null))
            {
                foreach (Tile neighbour in enemy.myTile.GetNeighbours())
                {
                    if (neighbour.IsLegalTile() && Pathfinder.Instance.WouldTileBeLegal(neighbour,myUnit,speed+2))
                    {
                        return neighbour.gameObject;
                    }
                }
            }
        }
        foreach (UnitScript enemy in VictoryLossChecker.GetEnemyUnitList())
        {
            if (Pathfinder.Instance.GetDistanceFromTo(myUnit.myTile, enemy.myTile, true) <= speed)
            {
                foreach (Tile neighbour in enemy.myTile.GetNeighbours())
                {
                    if (neighbour.IsLegalTile() && Pathfinder.Instance.WouldTileBeLegal(neighbour, myUnit, speed + 2))
                    {
                        return neighbour.gameObject;
                    }
                }
            }
        }
        // Here we have NO targets at all.. IMO we can move just by one for no reason at all xD
        foreach (Tile neighbour in myUnit.myTile.GetNeighbours())
        {
            if (neighbour.IsLegalTile())
            {
                Debug.Log(neighbour.name);
                return neighbour.gameObject;
            }
        }
        Debug.Log(myUnit.myTile.name);
        return myUnit.myTile.gameObject;
    }
}
