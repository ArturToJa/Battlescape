using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattlescapeLogic
{
    public class GroundMovement : AbstractMovement
    {
        //ctor
        public GroundMovement() : base()
        {
            visualSpeed = 1.5f;
        }

        public override IEnumerator MoveTo(Tile newPosition)
        {
            finalTile = newPosition;
            Queue<Tile> path = Pathfinder.instance.GetPathFromTo(myUnit, newPosition);
            BattlescapeGraphics.ColouringTool.UncolourAllTiles();
            PlayMovementAnimation();
            int tileCount = path.Count;
            for (int i = 0; i < tileCount; ++i)
            {               
                Tile temporaryGoal = path.Dequeue();                
                //I am aware, that for now we are still just turning into a direction in one frame. If we ever want it any other way, it needs a bit of work to set it otherwise so im not doing it now :D.                
                //if we want to slowly turn, we need to ask if we already turned, and if not we turn and if yes we move here.   
                TurnTowards(temporaryGoal.transform.position);
                while (Vector3.Distance(myUnit.transform.position, temporaryGoal.transform.position) > 0.0001f)
                {
                    myUnit.transform.position = Vector3.MoveTowards(myUnit.transform.position, temporaryGoal.transform.position,visualSpeed * Time.deltaTime);
                    yield return null;
                }
                myUnit.OnMove(myUnit.currentPosition, temporaryGoal);
                temporaryGoal.SetMyUnitTo(myUnit);
            }
            StopMovementAnimation();
            if (newPosition.IsProtectedByEnemyOf(myUnit))
            {
                myUnit.statistics.movementPoints = 0;
            }
            else
            {
                myUnit.statistics.movementPoints -= tileCount - 1;
            }
            BattlescapeGraphics.ColouringTool.ColourLegalTilesFor(myUnit);


        }

        void PlayMovementAnimation()
        {
            myUnit.animator.SetBool("Walking", true);
        }
        void StopMovementAnimation()
        {
            myUnit.animator.SetBool("Walking", false);
        }       
    }
}