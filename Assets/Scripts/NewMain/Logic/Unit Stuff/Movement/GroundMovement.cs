using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattlescapeLogic
{
    public class GroundMovement : AbstractMovement
    {
        float visualSpeed = 10f;
        //ctor
        public GroundMovement() : base()
        {
        }

        public override IEnumerator MoveTo(Tile newPosition)
        {           
            Queue<Tile> path = Pathfinder.instance.GetPathFromTo(myUnit, newPosition);
            ColouringTool.UncolourAllTiles();
            PlayMovementAnimation();
            int tileCount = path.Count;
            for (int i = 0; i < tileCount; ++i)
            {               
                Tile temporaryGoal = path.Dequeue();
                temporaryGoal.SetMyUnitTo(myUnit);
                //I am aware, that for now we are still just turning into a direction in one frame. If we ever want it any other way, it needs a bit of work to set it otherwise so im not doing it now :D.                
                //if we want to slowly turn, we need to ask if we already turned, and if not we turn and if yes we move here.   
                TurnTowards(temporaryGoal.transform.position);
                while (isMoving)
                {
                    myUnit.transform.position = Vector3.MoveTowards(myUnit.transform.position, temporaryGoal.transform.position,visualSpeed * Time.deltaTime);
                    yield return null;
                }
                
            }
            StopMovementAnimation();
            myUnit.statistics.movementPoints -= tileCount - 1;
            ColouringTool.ColourLegalTilesFor(myUnit);


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