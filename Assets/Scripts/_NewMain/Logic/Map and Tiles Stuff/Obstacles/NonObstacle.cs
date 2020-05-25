using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattlescapeLogic
{
    public class NonObstacle : MonoBehaviour, IOnTilePlaceable
    {
        [SerializeField] TileObjectType _type;
        public TileObjectType type
        {
            get
            {
                return _type;
            }
        }

        [SerializeField] protected MultiTile _currentPosition;
        public MultiTile currentPosition
        {
            get
            {
                return _currentPosition;
            }
            private set
            {
                _currentPosition = value;
            }
        }

        public void TryToSetMyPositionTo(Tile bottomLeftCorner)
        {
            SetMyPositionTo(CalibrateToFitInBoard(bottomLeftCorner));
        }

        void SetMyPositionTo(Tile newBottomLeftCorner)
        {
            currentPosition = MultiTile.Create(newBottomLeftCorner, currentPosition.width, currentPosition.height);
            this.transform.position = currentPosition.center;            
        }

        Tile CalibrateToFitInBoard(Tile bottomLeftCorner)
        {
            int posX = bottomLeftCorner.position.x;
            int posZ = bottomLeftCorner.position.z;
            if (posX + currentPosition.width > Global.instance.currentMap.mapWidth)
            {
                posX = Global.instance.currentMap.mapWidth - currentPosition.width;
            }
            if (posZ + currentPosition.height > Global.instance.currentMap.mapHeight)
            {
                posZ = Global.instance.currentMap.mapHeight - currentPosition.height;
            }
            return Global.instance.currentMap.board[posX, posZ];
        }

        public void OnSpawn(Tile spawningTile)
        {
            TryToSetMyPositionTo(spawningTile);
        }
    }
}