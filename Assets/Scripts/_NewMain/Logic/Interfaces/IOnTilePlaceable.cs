using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattlescapeLogic
{
    public interface IOnTilePlaceable
    {
        MultiTile currentPosition { get; }
    }
}

