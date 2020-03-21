using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattlescapeLogic
{
    //IDK if we need it or not but i didn't like forcing my physical transform.position (which is a float) into int all the time
    [System.Serializable]
    public struct Position
    {
        public Position(int _x, int _z)
        {
            x = _x;
            z = _z;
        }
        public int x;
        public int z;

        public int DistanceTo(Position other)
        {
            return Mathf.Max(Mathf.Abs(this.x - other.x), Mathf.Abs(this.z - other.z));
        }
    }

    public class Tile : MonoBehaviour, IMouseTargetable
    {
        public BattlescapeGraphics.TileHighlighter highlighter { get; private set; }
        public Unit myUnit { get; private set; }
        public Obstacle myObstacle { get; set; }
        public bool hasObstacle
        {
            get
            {
                return myObstacle != null;
            }
        }
        //this is not used as we, for some strange reason, dont show movement path!
        public bool isUnderMovementMarker { get; set; }
        public Position position { get; set; }
        [SerializeField] int _dropzoneOfTeam = -1;
        public int dropzoneOfTeam
        {
            get
            {
                return _dropzoneOfTeam;
            }
            set
            {
                _dropzoneOfTeam = value;
            }
        }
        //this needs changing to a) support more players b) support our Player class etc. Not for now i guess?
        //initialized in Map on line 42;

        public static event Action<Tile> OnMouseHoverTileEnter;        

        public List<Tile> neighbours
        {
            get
            {
                List<Tile> returnList = new List<Tile>();

                for (int i = 0; i < Global.instance.currentMap.mapWidth; i++)
                    for (int j = 0; j < Global.instance.currentMap.mapHeight; j++)
                    {
                        //next line means: all 8 neighbours, literally (and prevents OUR tile to be inside the scope), NOTE THAT unwalkable/obstacled tiles are STILL neighbours.
                        if (Mathf.Abs(i - position.x) <= 1 && Mathf.Abs(j - position.z) <= 1 && !(position.x == i && position.z == j))
                        {
                            returnList.Add(Global.instance.currentMap.board[i, j]);
                        }
                    }
                return returnList;
            }
        }

        public void OnSetup()
        {
            position = new Position((int)transform.position.x, (int)transform.position.z);
            Global.instance.currentMap.board[position.x, position.z] = this;
            highlighter = GetComponentInChildren<BattlescapeGraphics.TileHighlighter>();
        }

        public bool IsWalkable()
        {
            return myUnit == null && hasObstacle == false;
        }


        // This means 'Is this tile or a neighbour under enemy unit?'. It matters for movement (you cannot walk through tiles protected by enemy).
        // I just 'invented' the term 'protected' for that, I used 'occupied' earlier but it was VERY misleading.
        public bool IsProtectedByEnemyOf(BattlescapeLogic.Unit unit)
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
        bool IsTileUnderEnemyOfUnit(Tile tile, BattlescapeLogic.Unit unit)
        {
            return tile.myUnit != null && tile.myUnit.owner != unit.owner;
            //return tile.myUnit != null && tile.myUnit.owner.team != unit.owner.team;
            //^ this is correct for Unit;
        }

        //IDK how we want to do that but currently we do not deal with setting Units/Tiles in pairs AT ALL - we have no way to set those.
        public void SetMyUnitTo(Unit unit)
        {
            myUnit = unit;
            Tile oldTile = null;
            if (unit != null)
            {
                oldTile = myUnit.currentPosition;
                myUnit.currentPosition = this;
                if (oldTile != null && oldTile != this)
                {
                    oldTile.myUnit = null;
                }
            }
        }

        public void DestroyObstacle()
        {
            //Play some animations/sounds/stuff
            myObstacle = null;
        }

        public Tile GetMyTile()
        {
            return this;
        }

        public void OnMouseHoverEnter()
        {
            OnMouseHoverTileEnter(this);
        }

        public void OnMouseHoverExit()
        {
            foreach (Unit unit in Global.instance.GetAllUnits())
            {
                BattlescapeGraphics.ColouringTool.ColourObject(unit, Color.white);
            }
        }
    }
}

