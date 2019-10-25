using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BattlescapeLogic;

public class Pathfinder : MonoBehaviour
{
    int[,] Distances;
    public Tile[,] Parents;
    public bool[,] EnemyOccupations;

    public static Pathfinder Instance { get; private set; }

    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void BFS(UnitScript unit)
    {
        Parents = new Tile[Map.mapWidth, Map.mapHeight];
        SetDistancesToMinus();
        SetOccupations(unit);

        Queue<Tile> queue = new Queue<Tile>();
        Tile start = unit.myTile;
        int startX = Mathf.RoundToInt(start.transform.position.x);
        int startZ = Mathf.RoundToInt(start.transform.position.z);
        Distances[startX, startZ] = 0;
        queue.Enqueue(start);

        while (queue.Count > 0)
        {
            Tile current = queue.Peek();
            int currentX = Mathf.RoundToInt(current.transform.position.x);
            int currentZ = Mathf.RoundToInt(current.transform.position.z);
            queue.Dequeue();
            foreach (var neighbour in current.neighbours)
            {
                int X = Mathf.RoundToInt(neighbour.transform.position.x);
                int Z = Mathf.RoundToInt(neighbour.transform.position.z);
                if (Distances[X, Z] == -1 && !EnemyOccupations[X, Z])
                {
                    Distances[X, Z] = Distances[currentX, currentZ] + 1;
                    Parents[X, Z] = current;
                    queue.Enqueue(neighbour);
                }
                else if (Distances[X, Z] == -1 && EnemyOccupations[X, Z])
                {
                    Distances[X, Z] = Distances[currentX, currentZ] + 1;
                    Parents[X, Z] = current;
                }
            }
        }
    }

    private void SetOccupations(UnitScript selectedUnit)
    {
        EnemyOccupations = new bool[Map.mapWidth, Map.mapHeight];
        for (int i = 0; i < Map.mapWidth; i++)
        {
            for (int j = 0; j < Map.mapHeight; j++)
            {
                EnemyOccupations[i, j] = Map.Board[i, j].IsProtectedByEnemyOf(selectedUnit) || Map.Board[i, j].IsWalkable() == false || Map.Board[i, j].hasObstacle;
            }
        }
    }

    private void SetDistancesToMinus()
    {
        Distances = new int[Map.mapWidth, Map.mapHeight];
        for (int i = 0; i < Map.mapWidth; i++)
        {
            for (int j = 0; j < Map.mapHeight; j++)
            {
                Distances[i, j] = -1;
            }

        }
    }

    public void ColourPossibleTiles(UnitMovement unit, bool notQuittingCombat)
    {
        BFS(unit.GetComponent<UnitScript>());
        //TileColouringTool.UncolourAllTiles();
        //Debug.LogError(unit + " " + colourRed);
        for (int i = 0; i < Map.mapWidth; i++)
            for (int j = 0; j < Map.mapHeight; j++)
            {
                if (Distances[i, j] <= unit.GetCurrentMoveSpeed(notQuittingCombat) && Distances[i, j] != -1 && Map.Board[i, j].IsWalkable())
                {
                    if (notQuittingCombat || EnemyOccupations[i, j] == false)
                    {
                        if (Map.Board[i, j].IsProtectedByEnemyOf(unit.GetComponent<UnitScript>()))
                        {
                            ColouringTool.SetColour(Map.Board[i, j], Color.red);
                        }
                        else
                        {
                            ColouringTool.SetColour(Map.Board[i, j], Color.green);
                        }
                    }
                }
            }
                
    }



    public void DebugTile(Tile tile)
    {
        int X = Mathf.RoundToInt(tile.transform.position.x);
        int Z = Mathf.RoundToInt(tile.transform.position.z);
        if (Distances[X, Z] != -1)
        {
            Debug.Log(X + "_" + Z);
        }
        //Debug.Log("Distance of tile: " + X + " " + Z + Distances[X, Z]);
        // Debug.Log(Parents[X, Z]);
    }

    public int GetTilesX(Tile tile)
    {
        return Mathf.RoundToInt(tile.transform.position.x);
    }

    public int GetTilesZ(Tile tile)
    {
        return Mathf.RoundToInt(tile.transform.position.z);
    }

    public Dictionary<Tile, bool> GetAllLegalTilesAndIfTheyAreSafe(UnitMovement unit, bool isAffectedByCombat)
    {
        Dictionary<Tile, bool> theDictionary = new Dictionary<Tile, bool>();
        for (int i = 0; i < Map.mapWidth; i++)
            for (int j = 0; j < Map.mapHeight; j++)
                if (IsTileLegal(i, j, unit, isAffectedByCombat))
                {
                    theDictionary.Add(Map.Board[i, j], EnemyOccupations[i, j]);
                }
        //        Debug.Log("Unit: " + unit + " Tilecount: " + theDictionary.Count);
        return theDictionary;
    }
    bool IsTileLegal(int i, int j, UnitMovement unit, bool isAffectedByCombat)
    {
        return
               Distances[i, j] <= unit.GetCurrentMoveSpeed(!isAffectedByCombat)
            && Distances[i, j] != -1
            && Map.Board[i, j].IsWalkable() == true
            && Map.Board[i, j].hasObstacle == false
            && Map.Board[i, j].myUnit == null
            && Map.Board[i, j] != unit.GetComponent<UnitScript>().myTile
            && (EnemyOccupations[i, j] == false || unit.GetComponent<UnitScript>().CheckIfIsInCombat() == false || isAffectedByCombat == false);
    }

    public List<Tile> GetAllTilesThatWouldBeLegalIfNotInCombat(UnitScript unit, int speed)
    {
        List<Tile> theList = new List<Tile>();
        for (int i = 0; i < Map.mapWidth; i++)
            for (int j = 0; j < Map.mapHeight; j++)
                if (WouldTileBeLegal(Map.Board[i, j], unit, speed))
                {
                    theList.Add(Map.Board[i, j]);
                }
        //        Debug.Log("Unit: " + unit + " Tilecount: " + theDictionary.Count);
        return theList;
    }

    public bool WouldTileBeLegal(Tile tile, UnitScript unit, int speed)
    {
        BFS(unit);
        int i = Mathf.RoundToInt(tile.transform.position.x);
        int j = Mathf.RoundToInt(tile.transform.position.z);
        return
               Distances[i, j] <= speed
            && Distances[i, j] != -1
            && Map.Board[i, j].IsWalkable() == true
            && Map.Board[i, j].hasObstacle == false
            && Map.Board[i, j].myUnit == null
            && Map.Board[i, j] != unit.GetComponent<UnitScript>().myTile;

    }

    int GetDistance(Tile tile)
    {
        return Distances[Mathf.RoundToInt(tile.transform.position.x), Mathf.RoundToInt(tile.transform.position.z)];
    }
    public int GetDistanceFromTo(UnitScript start, Tile end)
    {
        BFS(start);
        if (end.myUnit == null)
        {
            return GetDistance(end);
        }
        else
        {
            List<Tile> neighbours = end.neighbours;
            int[] possibleAnswers = new int[neighbours.Count];

            for (int i = 0; i < neighbours.Count; i++)
            {
                if (neighbours[i].IsWalkable())
                {
                    possibleAnswers[i] = GetDistance(neighbours[i]);
                }
            }
            System.Array.Sort(possibleAnswers);
            return possibleAnswers[0];
        }

    }
}
