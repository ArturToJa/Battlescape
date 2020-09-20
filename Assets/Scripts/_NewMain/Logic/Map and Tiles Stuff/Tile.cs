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

        public Position CalibrateTo(int width, int height)
        {
            if (x < 0)
            {
                x = 0;
            }
            if (x >= Global.instance.currentMap.mapWidth - width + 1)
            {
                x = Global.instance.currentMap.mapWidth - width;
            }
            if (z < 0)
            {
                z = 0;
            }
            if (z >= Global.instance.currentMap.mapHeight - height + 1)
            {
                z = Global.instance.currentMap.mapHeight - height;
            }
            return this;
        }
    }

    public class Tile : MonoBehaviour, IMouseTargetable
    {
        public static readonly float width = 1.0f;
        public static readonly float height = 1.0f;

        public BattlescapeGraphics.TileHighlighter highlighter { get; private set; }
        OnTileObject myObject;

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
        // -1 means it is neutral, not a dropzone.

        //this needs changing to a) support more players b) support our Player class etc. Not for now i guess?
        //initialized in Map on line 42;


        List<Tile> _neighbours;
        public List<Tile> neighbours
        {
            get
            {
                if (_neighbours == null)
                {
                    _neighbours = new List<Tile>();
                    for (int i = -1; i <= 1; i++)
                    {
                        for (int j = -1; j <= 1; j++)
                        {
                            if (!(i == 0 && j == 0))
                            {
                                Tile neighbour = ToTile(Offset(i, j));
                                if (neighbour != null)
                                {
                                    _neighbours.Add(neighbour);
                                }
                            }
                        }
                    }
                }
                return _neighbours;
            }
        }
        public Position Offset(int offsetX, int offsetZ)
        {
            int posX = this.position.x + offsetX;
            int posZ = this.position.z + offsetZ;
            //if (posX >= 0 && posZ >= 0 && posX < Global.instance.currentMap.mapWidth && posZ < Global.instance.currentMap.mapHeight)
            {
                return new Position(posX, posZ);
            }


        }

        public static Tile ToTile(Position pos)
        {
            if (pos.x >= 0 && pos.z >= 0 && pos.x < Global.instance.currentMap.mapWidth && pos.z < Global.instance.currentMap.mapHeight)
            {
                return Global.instance.currentMap.board[pos.x, pos.z];
            }
            else
            {
                return null;
            }
        }

        public void OnSetup()
        {
            position = new Position((int)transform.position.x, (int)transform.position.z);
            Global.instance.currentMap.board[position.x, position.z] = this;
            highlighter = GetComponentInChildren<BattlescapeGraphics.TileHighlighter>();
            highlighter.OnSetup();
            if (dropzoneOfTeam != -1)
            {
                highlighter.TurnOn(Color.green);
            }
            else
            {
                highlighter.TurnOff();
            }
        }

        public bool IsWalkable()
        {
            return myObject == null;
        }

        public T GetMyObject<T>() where T : OnTileObject
        {
            return myObject as T;
        }

        public void SetMyObjectTo(OnTileObject anObject)
        {
            myObject = anObject;
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

        bool IsTileUnderEnemyOfUnit(Tile tile, Unit unit)
        {
            return tile.GetMyObject<Unit>() != null && tile.GetMyObject<Unit>().IsEnemyOf(unit);
            //return tile.GetMyObject<Unit>() != null && tile.GetMyObject<Unit>().GetMyOwner().team != unit.GetMyOwner().team;
            //^ this is correct for Unit;
        }

        public Tile GetMyTile()
        {
            return this;
        }

        public void OnMouseHoverEnter(Vector3 exactMousePosition)
        {
            if (GameRound.instance.currentPlayer.selectedUnit != null)
            {
                GameRound.instance.currentPlayer.selectedUnit.OnTileHovered(this, exactMousePosition);
            }
        }

        public void OnMouseHoverExit()
        {
            foreach (Unit unit in Global.instance.GetAllUnits())
            {
                BattlescapeGraphics.ColouringTool.ColourObject(unit, Color.white);
            }
        }

        public IDamageable GetMyDamagableObject()
        {
            return myObject as IDamageable;
        }

        public MultiTile PositionRelatedToMouse(int width, int height, Vector3 exactClickPosition)
        {
            // this variable is equal to 1 if width or height are even, otherwise 0
            int widthEven = width % 2;
            int heightEven = height % 2;

            // check which part of tile player clicked
            int xGt = Convert.ToInt32(exactClickPosition.x > this.transform.position.x);
            int zGt = Convert.ToInt32(exactClickPosition.z > this.transform.position.z);

            // find new bottom left corner of Multitile
            int widthOffset = (width - widthEven) / 2 - xGt * Convert.ToInt32(widthEven == 0);
            int heightOffset = (height - heightEven) / 2 - zGt * Convert.ToInt32(heightEven == 0);

            return MultiTile.Create(ToTile(this.Offset(-widthOffset, -heightOffset).CalibrateTo(width, height)), width, height);
        }
    }
}

