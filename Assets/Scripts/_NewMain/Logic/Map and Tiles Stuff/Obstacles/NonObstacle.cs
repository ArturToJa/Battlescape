using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattlescapeLogic
{
    public class NonObstacle : MonoBehaviour, IVisuals
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

        public void TryToSetMyPositionTo(MultiTile position)
        {
            SetMyPositionTo(CalibrateToFitInBoard(position));
        }

        void SetMyPositionTo(MultiTile newPosition)
        {
            currentPosition = newPosition;
            this.transform.position = currentPosition.center;
        }

        MultiTile CalibrateToFitInBoard(MultiTile position)
        {
            int posX = position.bottomLeftCorner.position.x;
            int posZ = position.bottomLeftCorner.position.z;
            if (posX + position.size.width > Global.instance.currentMap.mapWidth)
            {
                posX = Global.instance.currentMap.mapWidth - position.size.width;
            }
            if (posZ + position.size.height > Global.instance.currentMap.mapHeight)
            {
                posZ = Global.instance.currentMap.mapHeight - position.size.height;
            }
            return MultiTile.Create(Global.instance.currentMap.board[posX, posZ], position.size);
        }

        public void OnSpawn(MultiTile spawningTile)
        {
            TryToSetMyPositionTo(spawningTile);
        }
    }
}