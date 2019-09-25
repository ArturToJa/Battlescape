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

        int[,] Distances;
        Tile[,] Parents;
        bool[,] EnemyOccupations;



        //Using newly created "Tile" class just because there will anyway be a need for a new Tile class definitely ;) the old one is messy af...
        //Also - new Map class just for what old Map.Board[x,z] did.
        public Queue<Tile> GetPathFromTo(Unit unitToMove, Tile finalTile)
        {
            BFS(unitToMove);
            Stack<Tile> tileStack = new Stack<Tile>();
            tileStack.Push(finalTile);
            while (!tileStack.Contains(unitToMove.currentPosition))
            {
                Tile tileOnTheStack = tileStack.Peek();
                tileStack.Push(Parents[tileOnTheStack.position.x, tileOnTheStack.position.z]);
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
            Parents = new Tile[Map.instance.mapWidth, Map.instance.mapHeight];
            SetDistancesToMinus();
            SetOccupations(unitToMove);

            Queue<Tile> queue = new Queue<Tile>();
            Tile start = unitToMove.currentPosition;
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
        #region BFS Subfunctions
        void SetDistancesToMinus()
        {
            Distances = new int[Map.instance.mapWidth, Map.instance.mapHeight];
            for (int i = 0; i < Map.instance.mapWidth; i++)
            {
                for (int j = 0; j < Map.instance.mapHeight; j++)
                {
                    Distances[i, j] = -1;
                }

            }
        }
        //Tile is considered Occupied, if there is an enemy on it or on its neighbour.
        void SetOccupations(Unit unitToMove)
        {
            EnemyOccupations = new bool[Map.instance.mapWidth, Map.instance.mapHeight];
            for (int i = 0; i < Map.instance.mapWidth; i++)
            {
                for (int j = 0; j < Map.instance.mapHeight; j++)
                {
                    EnemyOccupations[i, j] = Map.instance.board[i, j].IsProtectedByEnemyOf(unitToMove) || Map.instance.board[i, j].IsWalkable() == false;
                }
            }
        }
        #endregion        
    }
}
