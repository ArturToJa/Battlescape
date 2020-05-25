using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattlescapeLogic
{
    public abstract class OnTileObject : TurnChangeMonoBehaviour, IOnTilePlaceable
    {    
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


        [SerializeField] TileObjectType _type;
        public TileObjectType type
        {
            get
            {
                return _type;
            }
        }

        public void TryToSetMyPositionTo(Tile bottomLeftCorner)
        {
            SetMyPositionTo(CalibrateToFitInBoard(bottomLeftCorner));
        }

        public void TryToSetMyPositionAndMoveTo(Tile bottomLeftCorner)
        {
            SetMyPositionAndMoveTo(CalibrateToFitInBoard(bottomLeftCorner));
        }

        void SetMyPositionTo(Tile newBottomLeftCorner)
        {            
            foreach (Tile tile in currentPosition)
            {
                tile.SetMyObjectTo(null);
            }
            currentPosition = MultiTile.Create(newBottomLeftCorner, currentPosition.width, currentPosition.height);
            foreach (Tile tile in currentPosition)
            {
                tile.SetMyObjectTo(this);
            }
        }

        void SetMyPositionAndMoveTo(Tile newBottomLeftCorner)
        {
            SetMyPositionTo(newBottomLeftCorner);
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
            TryToSetMyPositionAndMoveTo(spawningTile);
        }
    }


}