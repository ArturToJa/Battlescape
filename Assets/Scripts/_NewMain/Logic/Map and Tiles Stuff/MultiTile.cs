using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattlescapeLogic
{
    [System.Serializable]
    public class MultiTile : IEnumerable
    {
        public Tile bottomLeftCorner { get; private set; }
        [SerializeField] int _width = 1;
        public int width
        {
            get
            {
                return _width;
            }
            private set
            {
                _width = value;
            }
        }
        [SerializeField] int _height = 1;
        public int height
        {
            get
            {
                return _height;
            }
            private set
            {
                _height = value;
            }
        }

        [NonSerialized] List<MultiTile> _neighbours;
        public List<MultiTile> neighbours
        {
            get
            {
                if (_neighbours == null)
                {
                    _neighbours = new List<MultiTile>();
                    if (Offset(-1, 0) != null)
                    {
                        _neighbours.Add(Offset(-1, 0));
                    }
                    if (Offset(1, 0) != null)
                    {
                        _neighbours.Add(Offset(1, 0));
                    }
                    if (Offset(0, -1) != null)
                    {
                        _neighbours.Add(Offset(0, -1));
                    }
                    if (Offset(0, 1) != null)
                    {
                        _neighbours.Add(Offset(0, 1));
                    }


                    if (Offset(1, 1) != null)
                    {
                        _neighbours.Add(Offset(1, 1));
                    }
                    if (Offset(-1, 1) != null)
                    {
                        _neighbours.Add(Offset(-1, 1));
                    }
                    if (Offset(1, -1) != null)
                    {
                        _neighbours.Add(Offset(1, -1));
                    }
                    if (Offset(-1, -1) != null)
                    {
                        _neighbours.Add(Offset(-1, -1));
                    }
                }
                return _neighbours;
            }
        }

        //tiles literally next to this x by z tile group
        [NonSerialized] List<Tile> _closeNeighbours;
        public List<Tile> closeNeighbours
        {
            get
            {
                if (_closeNeighbours == null)
                {
                    _closeNeighbours = new List<Tile>();
                    Tile bottomLeftNeighbour = Tile.ToTile(bottomLeftCorner.Offset(-1, -1));
                    if (bottomLeftNeighbour != null)
                    {
                        _closeNeighbours.Add(bottomLeftNeighbour);
                    }
                    for (int i = 0; i < width; ++i)
                    {
                        Tile toAdd = Tile.ToTile(bottomLeftCorner.Offset(-1, i));
                        if (toAdd != null)
                        {
                            _closeNeighbours.Add(toAdd);
                        }
                        toAdd = Tile.ToTile(bottomLeftCorner.Offset(width, i));
                        if (toAdd != null)
                        {
                            _closeNeighbours.Add(toAdd);
                        }
                    }

                    for (int i = 0; i < height; ++i)
                    {
                        Tile toAdd = Tile.ToTile(bottomLeftCorner.Offset(i, -1));
                        if (toAdd != null)
                        {
                            _closeNeighbours.Add(toAdd);
                        }
                        toAdd = Tile.ToTile(bottomLeftCorner.Offset(i, height));
                        if (toAdd != null)
                        {
                            _closeNeighbours.Add(toAdd);
                        }
                    }
                    Tile topRightNeighbour = Tile.ToTile(bottomLeftCorner.Offset(width, height));
                    if (topRightNeighbour != null)
                    {
                        _closeNeighbours.Add(topRightNeighbour);
                    }
                }


                return _closeNeighbours;
            }
        }

        Vector3 _center;
        public Vector3 center
        {
            get
            {
                return bottomLeftCorner.transform.position + new Vector3((float)((width - 1.0f) / 2.0f), 0, (float)((height - 1.0f) / 2.0f));
            }
        }

        MultiTile(Tile _currentPosition, int _width, int _height)
        {
            bottomLeftCorner = _currentPosition;
            width = _width;
            height = _height;
        }

        public static MultiTile Create(Tile _currentPosition, int _width, int _height)
        {
            if (IsValidToCreate(_currentPosition, _width, _height))
            {
                return new MultiTile(_currentPosition, _width, _height);
            }
            else
            {
                return null;
            }
        }

        static bool IsValidToCreate(Tile _currentPosition, int _width, int _height)
        {
            if (_currentPosition != null)
            {
                for (int i = 0; i < _width; ++i)
                {
                    for (int j = 0; j < _height; ++j)
                    {
                        if (Tile.ToTile(_currentPosition.Offset(i, j)) == null)
                        {
                            return false;
                        }

                    }
                }
                return true;
            }

            return false;
        }

        public void SetMyObjectTo(OnTileObject newObject)
        {
            foreach (Tile tile in this)
            {
                tile.SetMyObjectTo(newObject);
            }
        }

        public IEnumerator<Tile> GetEnumerator()
        {
            if (bottomLeftCorner != null)
            {
                for (int i = 0; i < width; ++i)
                {
                    for (int j = 0; j < height; ++j)
                    {
                        yield return Global.instance.currentMap.board[bottomLeftCorner.position.x + i, bottomLeftCorner.position.z + j];
                    }
                }
            }

        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public MultiTile Offset(int offsetX, int offsetZ)
        {
            int posX = bottomLeftCorner.position.x + offsetX;
            int posZ = bottomLeftCorner.position.z + offsetZ;
            if (posX >= 0 && posZ >= 0 && posX + width <= Global.instance.currentMap.mapWidth && posZ + height <= Global.instance.currentMap.mapHeight)
            {
                Tile newBottomLeftCorner = Global.instance.currentMap.board[posX, posZ];
                return Create(newBottomLeftCorner, width, height);
            }
            else
            {
                return null;
            }

        }

        public bool IsWalkable()
        {
            foreach (Tile tile in this)
            {
                if (tile.IsWalkable() == false)
                {
                    return false;
                }
            }
            return true;
        }

        public bool IsFreeFor(OnTileObject onTileObject)
        {
            if (onTileObject.currentPosition.width != width || onTileObject.currentPosition.height != height)
            {
                Debug.LogError("This question should not be asked as this is not the right size of an OTP!");
                return false;
            }
            foreach (Tile tile in this)
            {
                if (tile.GetMyObject<OnTileObject>() != onTileObject && tile.IsWalkable() == false)
                {
                    return false;
                }
            }
            return true;
        }

        public bool IsProtectedByEnemyOf(Unit unit)
        {
            foreach (Tile tile in this)
            {
                if (tile.IsProtectedByEnemyOf(unit))
                {
                    return true;
                }
            }
            return false;
        }

        public bool IsDropzoneOfTeam(int teamIndex)
        {
            foreach (Tile tile in this)
            {
                if (tile.dropzoneOfTeam != teamIndex)
                {
                    return false;
                }
            }
            return true;
        }
    }
}