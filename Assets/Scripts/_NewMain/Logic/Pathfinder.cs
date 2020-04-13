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
        MultiTile[,] parents;
        bool[,] enemyProtection;

        //These are for us to always know if we BFSed from this tile standing in this position
        //So that we only BFS if we have new unit or unit moved.
        Unit lastUnit;
        Tile lastTile;

        bool HaveToBFSFor(Unit unitToMove)
        {
            return (lastUnit == unitToMove && lastTile == unitToMove.currentPosition.bottomLeftCorner) == false;
        }

        //This function gives the list of possible tiles a Unit could get to.
        public List<MultiTile> GetAllLegalPositionsFor(Unit unitToMove)
        {
            List<MultiTile> returnList = new List<MultiTile>();
            BFS(unitToMove);
            for (int i = 0; i < Global.instance.currentMap.mapWidth - unitToMove.currentPosition.width + 1; i++)
            {
                for (int j = 0; j < Global.instance.currentMap.mapHeight - unitToMove.currentPosition.height + 1; j++)
                {
                    MultiTile newPosition = MultiTile.Create(Global.instance.currentMap.board[i, j], unitToMove.currentPosition.width, unitToMove.currentPosition.height);
                    if (unitToMove.CanMoveTo(newPosition))
                    {
                        returnList.Add(newPosition);
                    }
                }
            }
            return returnList;
        }

        public bool IsLegalTileForUnit(MultiTile position, Unit unit)
        {
            BFS(unit);
            int legalDistance = 0;
            if (unit.IsInCombat() && unit.statistics.movementPoints > 0)
            {
                legalDistance = 1;
            }
            else if (unit.IsInCombat() == false)
            {
                legalDistance = unit.statistics.movementPoints;
            }
            return distances[position.bottomLeftCorner.position.x, position.bottomLeftCorner.position.z] <= legalDistance && distances[position.bottomLeftCorner.position.x, position.bottomLeftCorner.position.z] > 0;
        }

        public Queue<MultiTile> GetPathFromTo(Unit unitToMove, MultiTile destination)
        {
            BFS(unitToMove);
            Stack<MultiTile> tileStack = new Stack<MultiTile>();
            tileStack.Push(destination);
            while (!tileStack.Contains(unitToMove.currentPosition))
            {
                MultiTile positionOnTheStack = tileStack.Peek();
                tileStack.Push(parents[positionOnTheStack.bottomLeftCorner.position.x, positionOnTheStack.bottomLeftCorner.position.z]);
            }
            Queue<MultiTile> path = new Queue<MultiTile>();
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
                lastTile = unitToMove.currentPosition.bottomLeftCorner;
                lastUnit = unitToMove;
            }
            MultiTile start = unitToMove.currentPosition;
            parents = new MultiTile[Global.instance.currentMap.mapWidth - start.width + 1, Global.instance.currentMap.mapHeight - start.height + 1];
            SetDistancesToMinus(start);
            SetProtectedByEnemy(unitToMove);

            Queue<MultiTile> queue = new Queue<MultiTile>();
            distances[start.bottomLeftCorner.position.x, start.bottomLeftCorner.position.z] = 0;
            queue.Enqueue(start);

            while (queue.Count > 0)
            {
                MultiTile current = queue.Peek();
                queue.Dequeue();
                foreach (MultiTile neighbour in current.neighbours)
                {
                    if (distances[neighbour.bottomLeftCorner.position.x, neighbour.bottomLeftCorner.position.z] == -1 && neighbour.IsFreeFor(unitToMove) && IsQuittingCombatIntoCombat(unitToMove, neighbour) == false)
                    {
                        distances[neighbour.bottomLeftCorner.position.x, neighbour.bottomLeftCorner.position.z] = distances[current.bottomLeftCorner.position.x, current.bottomLeftCorner.position.z] + 1;
                        parents[neighbour.bottomLeftCorner.position.x, neighbour.bottomLeftCorner.position.z] = current;
                        foreach (Tile tile in neighbour)
                        {
                            if (!enemyProtection[tile.position.x, tile.position.z])
                            {
                                queue.Enqueue(neighbour);
                            }
                        }
                    }
                    //if(neighbour.bottomLeftCorner.position.DistanceTo(unitToMove.currentPosition.bottomLeftCorner.position) == 1)
                    //{
                    //    Debug.Log("x: " + neighbour.bottomLeftCorner.position.x + ", z:" + neighbour.bottomLeftCorner.position.z + " distances: " + distances[neighbour.bottomLeftCorner.position.x, neighbour.bottomLeftCorner.position.z]) ;
                    //}
                }
            }
        }

        bool IsQuittingCombatIntoCombat(Unit unitToMove, MultiTile position)
        {

            return unitToMove.IsInCombat() && position.IsProtectedByEnemyOf(unitToMove);
        }
        #region BFS Subfunctions
        void SetDistancesToMinus(MultiTile start)
        {
            distances = new int[Global.instance.currentMap.mapWidth - start.width + 1, Global.instance.currentMap.mapHeight - start.height + 1];
            for (int i = 0; i < Global.instance.currentMap.mapWidth - start.width + 1; i++)
            {
                for (int j = 0; j < Global.instance.currentMap.mapHeight - start.height + 1; j++)
                {
                    distances[i, j] = -1;
                }

            }
        }
        //Tile is considered Occupied, if there is an enemy on its neighbour.
        void SetProtectedByEnemy(Unit unitToMove)
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
        #endregion
    }
}
