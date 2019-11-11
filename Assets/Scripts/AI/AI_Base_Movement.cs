using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BattlescapeLogic;

public class AI_Base_Movement : AI_BaseClass
{
    public float ShootPen = -Mathf.Infinity;
    public float EnePen = -Mathf.Infinity;
    public float TargetBonus = -Mathf.Infinity;
    public float FutureBonus = -Mathf.Infinity;
    public float HighestScore = -Mathf.Infinity;

    public AI_Base_Movement(int ID) : base(ID)
    {
        base.CallTheConstructor(ID);
    }

    public override IEnumerator EvaluatePossibleMoves(BattlescapeLogic.Unit currentUnit, List<Tile> possibleMoves)
    {
        AI_Controller.isEvaluatingTiles = true;
        EvaluatedTiles = new Dictionary<Tile, float>();
        int temp = 0;
        int direction = FindDirectionOfEnemies(currentUnit);
        Dictionary<BattlescapeLogic.Unit, List<Tile>> EMR = GetAllEnemyMoves();

        foreach (var tile in possibleMoves)
        {
            temp++;
            EvaluatedTiles.Add(tile, EvaluateTile(currentUnit, tile, direction, EMR));
            //Debug.Log("Evaluate tile: " + tile.transform.position.x + " " + tile.transform.position.z);
            // Debug.Log("Evaluated as: " + EvaluatedTiles[tile]);
        }
        AI_Controller.isEvaluatingTiles = false;
        AI_Controller.tilesAreEvaluated = true;

        yield return null;

    }

    public int FindDirectionOfEnemies(BattlescapeLogic.Unit currentUnit)
    {
        int N = 0;
        int W = 0;
        int S = 0;
        int E = 0;
        foreach (BattlescapeLogic.Unit enemy in enemyList)
        {
            if (enemy.transform.position.x < currentUnit.transform.position.x)
            {
                W++;
            }
            if (enemy.transform.position.x > currentUnit.transform.position.x)
            {
                E++;
            }
            if (enemy.transform.position.z > currentUnit.transform.position.z)
            {
                N++;
            }
            if (enemy.transform.position.z < currentUnit.transform.position.z)
            {
                S++;
            }
        }
        if (N >= W)
        {
            if (N >= S)
            {
                if (N >= E)
                {
                    return 0;
                }
                else
                {
                    return 1;
                }
            }
            else
            {
                if (S >= E)
                {
                    return 2;
                }
                else
                {
                    return 1;
                }
            }
        }
        else
        {
            if (W >= S)
            {
                if (W >= E)
                {
                    return 3;
                }
                else
                {
                    return 1;
                }
            }
            else
            {
                if (S >= E)
                {
                    return 2;
                }
                else
                {
                    return 1;
                }
            }
        }
    }

    public virtual float EvaluateTile(BattlescapeLogic.Unit currentUnit, Tile tile, int enemiesDirection, Dictionary<BattlescapeLogic.Unit, List<Tile>> EnemiesMovementRanges)
    {
        // THIS has to be overriden!
        return Random.Range(-1f, 1f);
    }

    public override List<Tile> GetPossibleMoves(BattlescapeLogic.Unit unit, bool isAlly)
    {
        return new List<Tile>(Pathfinder.instance.GetAllLegalTilesFor(unit));
    }

    public Dictionary<BattlescapeLogic.Unit, List<Tile>> GetAllEnemyMoves()
    {
        Dictionary<BattlescapeLogic.Unit, List<Tile>> AllEnemyMovesDictionary = new Dictionary<BattlescapeLogic.Unit, List<Tile>>();
        foreach (BattlescapeLogic.Unit enemy in enemyList)
        {
            List<Tile> tilesInDanger = GetPossibleMoves(enemy, false);
            AllEnemyMovesDictionary.Add(enemy,tilesInDanger);
        }
        return AllEnemyMovesDictionary;
    }
}
