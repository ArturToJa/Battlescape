using System;
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
        Unit lastUnit;
        Tile lastTile;

        bool HaveToBFSFor(Unit unitToMove)
        {
            return (lastUnit == unitToMove && lastTile == unitToMove.currentPosition) == false;
        }

        //Some old ability wants this ;D
        public int GetDistanceFromTo(Unit unit, Tile tile)
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
        public List<Tile> GetAllLegalTilesFor(Unit unitToMove)
        {
            List<Tile> returnList = new List<Tile>();
            // DOES NOT NEED TO  BFS HERE as it does BFS in each IsLegalTileForUnit but maybe one day it will not so remember it has to BFS Somewhere!
            //Also it has to BFS there as it is also used elsewhere;
            BFS(unitToMove);
            foreach (Tile tile in Global.instance.currentMap.board)
            {
                if (IsLegalTileForUnit(tile, unitToMove))
                {
                    returnList.Add(tile);
                }
            }
            return returnList;
        }

        public bool IsLegalTileForUnit(Tile tile, Unit unit)
        {
            BFS(unit);
            int legalDistance = 0;
            if (unit.IsInCombat() && unit.statistics.movementPoints > 0)
            {
                legalDistance = 1;
            }
            if (unit.IsInCombat() == false)
            {
                legalDistance = unit.statistics.movementPoints;
            }
            return distances[tile.position.x, tile.position.z] <= legalDistance && distances[tile.position.x, tile.position.z] > 0;
        }

        public Queue<Tile> GetPathFromTo(Unit unitToMove, Tile finalTile)
        {
            BFS(unitToMove);
            Stack<Tile> tileStack = new Stack<Tile>();
            tileStack.Push(finalTile);
            while (!tileStack.Contains(unitToMove.currentPosition))
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
        void BFS(Unit unitToMove)
        {
            //THIS first part is just for optimization
            if (HaveToBFSFor(unitToMove) == false)
            {
                return;
            }
            else
            {
                lastTile = unitToMove.currentPosition;
                lastUnit = unitToMove;
            }
            parents = new Tile[Global.instance.currentMap.mapWidth, Global.instance.currentMap.mapHeight];
            SetDistancesToMinus();
            SetOccupations(unitToMove);

            Queue<Tile> queue = new Queue<Tile>();
            Tile start = unitToMove.currentPosition;
            distances[start.position.x, start.position.z] = 0;
            queue.Enqueue(start);

            while (queue.Count > 0)
            {
                Tile current = queue.Peek();
                queue.Dequeue();
                List<Tile> orderedNeighbours = OrderNeighbours(current);
                foreach (Tile neighbour in orderedNeighbours)
                {                    
                    if (distances[neighbour.position.x, neighbour.position.z] == -1 && neighbour.IsWalkable() && IsQuittingCombatIntoCombat(unitToMove, neighbour) == false)
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

        bool IsQuittingCombatIntoCombat(Unit unitToMove, Tile tile)
        {
            return unitToMove.IsInCombat() && tile.IsProtectedByEnemyOf(unitToMove);
        }
        #region BFS Subfunctions
        void SetDistancesToMinus()
        {
            distances = new int[Global.instance.currentMap.mapWidth, Global.instance.currentMap.mapHeight];
            for (int i = 0; i < Global.instance.currentMap.mapWidth; i++)
            {
                for (int j = 0; j < Global.instance.currentMap.mapHeight; j++)
                {
                    distances[i, j] = -1;
                }

            }
        }
        //Tile is considered Occupied, if there is an enemy on its neighbour.
        void SetOccupations(BattlescapeLogic.Unit unitToMove)
        {
            enemyProtection = new bool[Global.instance.currentMap.mapWidth, Global.instance.currentMap.mapHeight];
            for (int i = 0; i < Global.instance.currentMap.mapWidth; i++)
            {
                for (int j = 0; j < Global.instance.currentMap.mapHeight; j++)
                {
                    enemyProtection[i, j] = Global.instance.currentMap.board[i, j].IsProtectedByEnemyOf(unitToMove);
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
