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
        //no idea if this should be private set or not... Or will we need this to be even public at all! I cannot design code well :<

        public Unit myUnit { get; private set; }
        bool hasObstacle;
        public Position position;

        public List<Tile> neighbours
        {
            get
            {
                List<Tile> priorityList = new List<Tile>();
                List<Tile> temporaryList = new List<Tile>();
                //I do not remember, WHY the two lists are needed, but i THINK it was so that the pathfinder prioritizes moving horizontally or vertically over diagonal movement.
                //That allows more 'human-acceptably' looking moves (no diagonal wiggling where straight line can be used).                
                //For that reason, the tiles with either X or Z (i and j) value being the same (i.e. in the same row or column) have PRIORITY in getting to the list above diagonally connected tiles.

                for (int i = 0; i < Map.instance.mapWidth; i++)
                    for (int j = 0; j < Map.instance.mapHeight; j++)
                    {
                        //next line means: all 8 neighbours, literally (and prevents OUR tile to be inside the scope), NOTE THAT unwalkable/obstacled tiles are STILL neighbours.
                        if (Mathf.Abs(i - position.x) <= 1 && Mathf.Abs(j - position.z) <= 1 && !(position.x == i && position.z == j))
                        {
                            //What im doinng here most likely can be done by just sorting the list and then two lists are redundant. I know.
                            //Sadly, I never attended ANY programming course so I can make a playable game but cannot sort a list. ;<
                            //We add the "priority" tiles to the priority list first and store the rest in the temporary list
                            if (position.x == i || position.z == j)
                            {
                                priorityList.Add(Map.instance.board[i, j]);
                            }
                            else
                            {
                                temporaryList.Add(Map.instance.board[i, j]);
                            }

                        }
                    }
                //THEN we add the temporary list into the prio list and return it.
                foreach (Tile tile in temporaryList)
                {
                    priorityList.Add(tile);
                }
                return priorityList;
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

