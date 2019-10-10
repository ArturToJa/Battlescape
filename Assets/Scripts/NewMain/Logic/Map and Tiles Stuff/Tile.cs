using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattlescapeLogic
{
    //IDK if we need it or not but i didn't like forcing my physical transform.position (which is a float) into int all the time
    public struct Position
    {
        public Position(int _x, int _z)
        {
            x = _x;
            z = _z;
        }
        public int x;
        public int z;
    }
    public class Tile : MonoBehaviour
    {
        // put correct width and height here
        public static float tileWidth = 0;
        public static float tileHeight = 0;
        public Unit myUnit { get; private set; }
        public bool hasObstacle { get; private set; }
        public Position position;

        public List<Tile> neighbours
        {
            get
            {
                List<Tile> returnList = new List<Tile>();

                for (int i = 0; i < Map.instance.mapWidth; i++)
                    for (int j = 0; j < Map.instance.mapHeight; j++)
                    {
                        //next line means: all 8 neighbours, literally (and prevents OUR tile to be inside the scope), NOTE THAT unwalkable/obstacled tiles are STILL neighbours.
                        if (Mathf.Abs(i - position.x) <= 1 && Mathf.Abs(j - position.z) <= 1 && !(position.x == i && position.z == j))
                        {
                            returnList.Add(Map.instance.board[i, j]);
                        }
                    }                
                return returnList;
            }
        }


        //this should JUST give info, if there is ANY CHANCE that ANY unit (does not care about who the owner is) can finish movement here/walk through it without abilities/flying.
        //at least thats what i think...    
        public bool IsWalkable()
        {
            return myUnit == null && hasObstacle == false;
        }


        // This means 'Is this tile or a neighbour under enemy unit?'. It matters for movement (you cannot walk through tiles protected by enemy).
        // I just 'invented' the term 'protected' for that, I used 'occupied' earlier but it was VERY misleading.
        public bool IsProtectedByEnemyOf(Unit unit)
        {
            if (IsTileUnderEnemyOfUnit(this, unit))
            {
                return true;
            }
            foreach (Tile tile in neighbours)
            {
                if (IsTileUnderEnemyOfUnit(tile, unit))
                {
                    return true;
                }
            }
            return false;
        }

        //I don't know if this function should exist HERE or if it should even exist at all, but it just makes stuff easier to read.
        bool IsTileUnderEnemyOfUnit(Tile tile, Unit unit)
        {
            return tile.myUnit != null && tile.myUnit.owner.team != unit.owner.team;
        }

    }
}

