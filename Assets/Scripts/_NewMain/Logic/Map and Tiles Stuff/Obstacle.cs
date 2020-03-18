using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattlescapeLogic
{
    public class Obstacle : OnTileObject, IMouseTargetable
    {
        [SerializeField] List<Position> _shape;
        public List<Position> shape
        {
            get
            {
                return _shape;
            }
            set
            {
                _shape = value;
            }
        }        

        public int GetDistanceFrom(Tile other)
        {
            int answer = 9999999;
            foreach (Tile tile in myTiles)
            {
                int distance = tile.position.DistanceTo(other.position);
                if (distance < answer)
                {
                    answer = distance;
                }
            }
            return answer;
        }

        

        public override void OnSpawn(Tile spawningTile)
        {
            SetMyTiles(spawningTile);
            SetAsObstacleForMyTiles();
        }


        void SetAsObstacleForMyTiles()
        {
            foreach (Tile tile in myTiles)
            {
                tile.myObstacle = this;
            }
        }

        void SetMyTiles(Tile middle)
        {
            myTiles = new List<Tile>();
            foreach (Position position in shape)
            {
                if (middle.position.x + position.x >= 0 &&
                    middle.position.x + position.x < Global.instance.currentMap.mapWidth &&
                    middle.position.z + position.z >= 0 &&
                    middle.position.z + position.z < Global.instance.currentMap.mapHeight)
                {
                    myTiles.Add(Global.instance.currentMap.board[middle.position.x + position.x, middle.position.z + position.z]);
                }
            }
        }

        public virtual void OnMouseHoverEnter()
        {
            return;
        }

        public virtual void OnMouseHoverExit()
        {
            return;
        }
    }
}