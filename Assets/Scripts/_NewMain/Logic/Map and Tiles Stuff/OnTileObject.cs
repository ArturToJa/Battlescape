using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattlescapeLogic
{
    public class OnTileObject : MonoBehaviour
    {

        [SerializeField] TileObjectType _type;
        public TileObjectType type
        {
            get
            {
                return _type;
            }
            private set
            {
                _type = value;
            }
        }

        public List<Tile> myTiles { get; protected set; } 
        

        public virtual void OnSpawn(Tile spawningTile)
        {
            return;
        }
    }
}
public enum TileObjectType
{
    Mushroom, Grass, FloorStone, Rock, SmallTree, FourTileSquare, Fence, Box, Barell
}

