using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattlescapeLogic
{
    public interface IOnTilePlaceable
    {
        string name { get; }
        TileObjectType type { get; }

        Tile[,] currentPosition { get; }


        void OnSpawn(Tile spawningTile);       
    }
}
public enum TileObjectType
{
    Mushroom, Grass, FloorStone, Rock, SmallTree, FourTileSquare, Fence, Box, Barell, Other
}

