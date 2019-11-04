using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BattlescapeLogic
{
    public class Pathfinder
    {
        static Pathfinder _instance;
        public static Pathfinder instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new Pathfinder();
                }
                return _instance;
            }
        }

        int[,] distances;
        Tile[,] parents;
        bool[,] enemyProtection;
        
        //These are for us to always know if we BFSed from this tile standing in this position
        //So that we only BFS if we have new unit or unit moved.
        UnitScript lastUnit;
        Tile lastTile;

        bool HaveToBFSFor(UnitScript unitToMove)
        {
            return (lastUnit == unitToMove && lastTile == unitToMove.myTile) == false;
        }

        //Some old ability wants this ;D
        public int GetDistanceFromTo(UnitScript unit, Tile tile)
        {
            int distance = distances[tile.position.x, tile.position.z];
            BFS(unit);
            if (distance == -1)
            {
                //THIS should never occur I THINK but maybe will.
                Debug.LogWarning("Not sure if legal");
                return 9999;
            }
            return distance;
        }
        
        

        //This function gives the list of possible tiles a Unit could get to.
        public List<Tile> GetAllLegalTilesFor(UnitScript unitToMove)
        {
            List<Tile> returnList = new List<Tile>();
            // DOES NOT NEED TO  BFS HERE as it does BFS in each IsLegalTileForUnit but maybe one day it will not so remember it has to BFS Somewhere!
            //Also it has to BFS there as it is also used elsewhere;
            BFS(unitToMove);
            foreach (Tile tile in Map.Board)
            {
                if (IsLegalTileForUnit(tile, unitToMove))
                {
                    returnList.Add(tile);
                }
            }
            return returnList;
        }

        public bool IsLegalTileForUnit(Tile tile, UnitScript unit)
        {
            BFS(unit);
            return distances[tile.position.x, tile.position.z] <= unit.statistics.movementPoints && distances[tile.position.x, tile.position.z] > 0;
        }

        public Queue<Tile> GetPathFromTo(UnitScript unitToMove, Tile finalTile)
        {
            BFS(unitToMove);
            Stack<Tile> tileStack = new Stack<Tile>();
            tileStack.Push(finalTile);
            while (!tileStack.Contains(unitToMove.myTile))
            {
                Tile tileOnTheStack = tileStack.Peek();
                tileStack.Push(parents[tileOnTheStack.position.x, tileOnTheStack.position.z]);
            }
            Queue<Tile> path = new Queue<Tile>();
            while (tileStack.Count > 0)
            {
                path.Enqueue(tileStack.Pop());
            }
            return path;
        }


        //This function populates Distances and Parents arrays with data, using BFS algorithm.
        //Currently it just calculates for whole board (not until reaching destination).
        void BFS(UnitScript unitToMove)
        {
            //THIS first part is just for optimization
            if (HaveToBFSFor(unitToMove) == false)
            {
                return;
            }
            else
            {
                lastTile = unitToMove.myTile;
                lastUnit = unitToMove;
            }
            parents = new Tile[Map.mapWidth, Map.mapHeight];
            SetDistancesToMinus();
            SetOccupations(unitToMove);

            Queue<Tile> queue = new Queue<Tile>();
            Tile start = unitToMove.myTile;
            distances[start.position.x, start.position.z] = 0;
            queue.Enqueue(start);

            while (queue.Count > 0)
            {
                Tile current = queue.Peek();
                queue.Dequeue();
                List<Tile> orderedNeighbours = OrderNeighbours(current);
                foreach (var neighbour in orderedNeighbours)
                {                    
                    if (distances[neighbour.position.x, neighbour.position.z] == -1 && neighbour.IsWalkable())
                    {
                        distances[neighbour.position.x, neighbour.position.z] = distances[current.position.x, current.position.z] + 1;
                        parents[neighbour.position.x, neighbour.position.z] = current;
                        if (!enemyProtection[neighbour.position.x, neighbour.position.z])
                        {
                            queue.Enqueue(neighbour);
                        }
                    }
                }
            }
        }
        #region BFS Subfunctions
        void SetDistancesToMinus()
        {
            distances = new int[Map.mapWidth, Map.mapHeight];
            for (int i = 0; i < Map.mapWidth; i++)
            {
                for (int j = 0; j < Map.mapHeight; j++)
                {
                    distances[i, j] = -1;
                }

            }
        }
        //Tile is considered Occupied, if there is an enemy on its neighbour.
        void SetOccupations(UnitScript unitToMove)
        {
            enemyProtection = new bool[Map.mapWidth, Map.mapHeight];
            for (int i = 0; i < Map.mapWidth; i++)
            {
                for (int j = 0; j < Map.mapHeight; j++)
                {
                    enemyProtection[i, j] = Map.Board[i, j].IsProtectedByEnemyOf(unitToMove);
                }
            }
        }

        List<Tile> OrderNeighbours(Tile tile)
        {
            //Basically sorts the neighbours tile in the way that the vertical/horizontal have prio over diagonal. Allows for more natural-looking paths.
            List<Tile> returnList = new List<Tile>();
            foreach (Tile neighbour in tile.neighbours)
            {
                if (neighbour.position.x == tile.position.x || neighbour.position.z == tile.position.z)
                {
                    returnList.Add(neighbour);
                }
            }
            foreach (Tile neighbour in tile.neighbours)
            {
                if (returnList.Contains(neighbour) == false)
                {
                    returnList.Add(neighbour);
                }
            }
            return returnList;
        }
        #endregion        
    }
}
