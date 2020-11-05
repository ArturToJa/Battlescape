using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattlescapeLogic
{
    public interface IVisuals : IOnTilePlaceable
    {
        string name { get; }
        TileObjectType type { get; }
        void OnSpawn(MultiTile spawningTile);
    }


    public enum TileObjectType
    {
        Mushroom, Grass, FloorStone, Rock, SmallTree, FourTileSquare, Fence, Box, Barell, Other
    }
}