using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattlescapeLogic
{
    public class TeleportationMovement : AbstractMovement
    {
        public override IEnumerator MoveTo(MultiTile destination)
        {
            Debug.Log("Woof");
            BattlescapeGraphics.ColouringTool.UncolourAllTiles();
            myUnit.transform.position = destination.center;
            myUnit.visuals.transform.position = destination.center;
            MultiTile oldTile = myUnit.currentPosition;
            myUnit.TryToSetMyPositionTo(destination);
            myUnit.OnMove(oldTile, destination);
            destination.SetMyObjectTo(myUnit);
            yield return null;
        }
    }
}