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
        bool[,] enemyOccupations;



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
            parents = new Tile[NewMap.instance.mapWidth, NewMap.instance.mapHeight];
            SetDistancesToMinus();
            SetOccupations(unitToMove);

            Queue<Tile> queue = new Queue<Tile>();
            Tile start = unitToMove.currentPosition;
            int startX = Mathf.RoundToInt(start.transform.position.x);
            int startZ = Mathf.RoundToInt(start.transform.position.z);
            distances[startX, startZ] = 0;
            queue.Enqueue(start);

            while (queue.Count > 0)
            {
                Tile current = queue.Peek();
                int currentX = Mathf.RoundToInt(current.transform.position.x);
                int currentZ = Mathf.RoundToInt(current.transform.position.z);
                queue.Dequeue();
                List<Tile> orderedNeighbours = OrderNeighbours(current);
                foreach (var neighbour in orderedNeighbours)
                {
                    int X = Mathf.RoundToInt(neighbour.transform.position.x);
                    int Z = Mathf.RoundToInt(neighbour.transform.position.z);
                    if (distances[X, Z] == -1 && !enemyOccupations[X, Z])
                    {
                        distances[X, Z] = distances[currentX, currentZ] + 1;
                        parents[X, Z] = current;
                        queue.Enqueue(neighbour);
                    }
                    else if (distances[X, Z] == -1 && enemyOccupations[X, Z])
                    {
                        distances[X, Z] = distances[currentX, currentZ] + 1;
                        parents[X, Z] = current;
                    }
                }
            }
        }
        #region BFS Subfunctions
        void SetDistancesToMinus()
        {
            distances = new int[NewMap.instance.mapWidth, NewMap.instance.mapHeight];
            for (int i = 0; i < NewMap.instance.mapWidth; i++)
            {
                for (int j = 0; j < NewMap.instance.mapHeight; j++)
                {
                    distances[i, j] = -1;
                }

            }
        }
        //Tile is considered Occupied, if there is an enemy on it or on its neighbour.
        void SetOccupations(Unit unitToMove)
        {
            enemyOccupations = new bool[NewMap.instance.mapWidth, NewMap.instance.mapHeight];
            for (int i = 0; i < NewMap.instance.mapWidth; i++)
            {
                for (int j = 0; j < NewMap.instance.mapHeight; j++)
                {
                    //enemyOccupations[i, j] = NewMap.instance.board[i, j].IsProtectedByEnemyOf(unitToMove) || NewMap.instance.board[i, j].IsWalkable() == false;
                    //this makes SENSE, but is impossible for as long as Tile uses UnitScript not Unit as it should ;) we still use old Pathfinder so this is OK?
                }
            }
        }

        List<Tile> OrderNeighbours(Tile tile)
        {
            //Basically sorts the neighbours tile in the way that the vertical/horizontal have prio over diagonal. Allows for more natural-looking paths.
            List<Tile> returnList = new List<Tile>();
            foreach (Tile neighbour in tile.neighbours)
            {
                if (neighbour.position.x == tile.position.x ||neighbour.position.z == tile.position.z)
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
