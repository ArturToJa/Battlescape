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

    //WE USE THIS! This is our TILE! Old "TILE" is no longer a thing!
    //Temporarily(!) uses UnitScript, not Unit. :< NEEDS CHANGING ASAP!
    public class Tile : MonoBehaviour
    {
        // put correct width and height here
        public static float tileWidth = 0;
        public static float tileHeight = 0;
        public UnitScript myUnit { get; private set; }
        public GameObject myObstacle { get; set; }
        public bool hasObstacle
        {
            get
            {
                return myObstacle != null;
            }
        }
        public bool isUnderMovementMarker { get; set; }
        public Position position { get; set; }
        public bool[] isDropzoneOfPlayer { get; set; }
        //this needs changing to a) support more players b) support our Player class etc. Not for now i guess?
        //initialized in Map on line 42;

        public List<Tile> neighbours
        {
            get
            {
                List<Tile> returnList = new List<Tile>();

                for (int i = 0; i < Map.mapWidth; i++)
                    for (int j = 0; j < Map.mapHeight; j++)
                    {
                        //next line means: all 8 neighbours, literally (and prevents OUR tile to be inside the scope), NOTE THAT unwalkable/obstacled tiles are STILL neighbours.
                        if (Mathf.Abs(i - position.x) <= 1 && Mathf.Abs(j - position.z) <= 1 && !(position.x == i && position.z == j))
                        {
                            returnList.Add(Map.Board[i, j]);
                        }
                    }
                return returnList;
            }
        }


        private void Start()
        {
            position = new Position((int)transform.position.x, (int)transform.position.z);
        }

        //this should JUST give info, if there is ANY CHANCE that ANY unit (does not care about who the owner is) can finish movement here/walk through it without abilities/flying.
        //at least thats what i think...    
        public bool IsWalkable()
        {
            return myUnit == null && hasObstacle == false;
        }


        // This means 'Is this tile or a neighbour under enemy unit?'. It matters for movement (you cannot walk through tiles protected by enemy).
        // I just 'invented' the term 'protected' for that, I used 'occupied' earlier but it was VERY misleading.
        public bool IsProtectedByEnemyOf(UnitScript unit)
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

        //TEMPORARY function, I think?

        //I don't know if this function should exist HERE or if it should even exist at all, but it just makes stuff easier to read.
        bool IsTileUnderEnemyOfUnit(Tile tile, UnitScript unit)
        {
            return tile.myUnit != null && tile.myUnit.PlayerID != unit.PlayerID;
            //return tile.myUnit != null && tile.myUnit.owner.team != unit.owner.team;
            //^ this is correct for Unit;
        }

        //IDK how we want to do that but currently we do not deal with setting Units/Tiles in pairs AT ALL - we have no way to set those.
        public void SetMyUnitTo(UnitScript unit)
        {
            myUnit = unit;
            Tile oldTile = null;
            if (unit != null)
            {
                 oldTile = myUnit.myTile;
                if (oldTile != null && oldTile != this)
                {
                    oldTile.myUnit = null;
                    myUnit.myTile = this;
                }
            }                        
        }

        public void DestroyObstacle()
        {
            //Play some animations/sounds/stuff
            myObstacle = null;
        }

    }
}

