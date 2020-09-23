using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattlescapeLogic
{
    [System.Serializable]
    public struct Size
    {
        public int width;
        public int height;

        public Size(int width, int height)
        {
            this.width = width;
            this.height = height;
        }

        public static bool operator ==(Size s1, Size s2)
        {
            return s1.width == s2.width && s1.height == s2.height;
        }

        public static bool operator !=(Size s1, Size s2)
        {
            return s1.width != s2.width || s1.height != s2.height;
        }
    }








    [System.Serializable]
    public class MultiTile : IEnumerable
    {
        public Tile bottomLeftCorner { get; private set; }
        [SerializeField] Size _size;
        public Size size
        {
            get
            {
                return _size;
            }
            private set
            {
                _size = value;
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
                    for (int i = 0; i < size.width; ++i)
                    {
                        Tile toAdd = Tile.ToTile(bottomLeftCorner.Offset(-1, i));
                        if (toAdd != null)
                        {
                            _closeNeighbours.Add(toAdd);
                        }
                        toAdd = Tile.ToTile(bottomLeftCorner.Offset(size.width, i));
                        if (toAdd != null)
                        {
                            _closeNeighbours.Add(toAdd);
                        }
                    }

                    for (int i = 0; i < size.height; ++i)
                    {
                        Tile toAdd = Tile.ToTile(bottomLeftCorner.Offset(i, -1));
                        if (toAdd != null)
                        {
                            _closeNeighbours.Add(toAdd);
                        }
                        toAdd = Tile.ToTile(bottomLeftCorner.Offset(i, size.height));
                        if (toAdd != null)
                        {
                            _closeNeighbours.Add(toAdd);
                        }
                    }
                    Tile topRightNeighbour = Tile.ToTile(bottomLeftCorner.Offset(size.width, size.height));
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
                return bottomLeftCorner.transform.position + new Vector3((float)((size.width - 1.0f) / 2.0f), 0, (float)((size.height - 1.0f) / 2.0f));
            }
        }

        MultiTile(Tile _currentPosition, Size _size)
        {
            bottomLeftCorner = _currentPosition;
            size = _size;
        }

        public static MultiTile Create(Tile _currentPosition, Size _size)
        {
            if (IsValidToCreate(_currentPosition, _size))
            {
                return new MultiTile(_currentPosition, _size);
            }
            else
            {
                return null;
            }
        }

        static bool IsValidToCreate(Tile _currentPosition, Size _size)
        {
            if (_currentPosition != null)
            {
                for (int i = 0; i < _size.width; ++i)
                {
                    for (int j = 0; j < _size.height; ++j)
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

        public void SetMyNonObstacleTo(NonObstacle newObject)
        {
            foreach (Tile tile in this)
            {
                tile.myNonObstacle =newObject;
            }
        }

        public IEnumerator<Tile> GetEnumerator()
        {
            if (bottomLeftCorner != null)
            {
                for (int i = 0; i < size.width; ++i)
                {
                    for (int j = 0; j < size.height; ++j)
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
            if (posX >= 0 && posZ >= 0 && posX + size.width <= Global.instance.currentMap.mapWidth && posZ + size.height <= Global.instance.currentMap.mapHeight)
            {
                Tile newBottomLeftCorner = Global.instance.currentMap.board[posX, posZ];
                return Create(newBottomLeftCorner, size);
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

/// <summary>
/// Use this function to find out if there is ANYTHING (either unit, obstacle or just e.g. mushroom) on this tile already. Mostly usefull for map generator (no duplicate objects on one tile). In most cases use IsWalkable instead.
/// </summary>
/// <returns></returns>
        public bool IsEmpty()
        {
            foreach (Tile tile in this)
            {
                if (tile.IsWalkable() == false || tile.myNonObstacle != null)
                {
                    return false;
                }
            }
            return true;
        }

        public bool IsFreeFor(OnTileObject onTileObject)
        {
            if (onTileObject.currentPosition.size != size)
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

        public int DistanceTo(MultiTile other)
        {
            int currentDistance = Int32.MaxValue;
            foreach (Tile tile in this)
            {
                foreach (Tile otherTile in other)
                {
                    int newDistance = tile.position.DistanceTo(otherTile.position);
                    if (newDistance < currentDistance)
                    {
                        currentDistance = newDistance;
                    }
                }
            }
            return currentDistance;
        }

        public int DistanceTo(Tile other)
        {
            int currentDistance = Int32.MaxValue;
            foreach (Tile tile in this)
            {
                int newDistance = tile.position.DistanceTo(other.position);
                if (newDistance < currentDistance)
                {
                    currentDistance = newDistance;
                }
            }
            return currentDistance;
        }
    }
}