using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattlescapeLogic
{
    public class GroundMovement : AbstractMovement
    {
        float visualSpeed;
        //ctor
        public GroundMovement() : base()
        {
        }

        public override IEnumerator MoveTo(Tile newPosition)
        {
            PlayMovementAnimation();
            Queue<Tile> path = Pathfinder.instance.GetPathFromTo(myUnit, newPosition);
            for (int i = 0; i < path.Count; ++i)
            {
                Tile temporaryGoal = path.Dequeue();
                //I am aware, that for now we are still just turning into a direction in one frame. If we ever want it any other way, it needs a bit of work to set it otherwise so im not doing it now :D.                
                myUnit.currentPosition = temporaryGoal;
                while (isMoving)
                {
                    VisuallyMoveTowards(temporaryGoal);
                    yield return null;
                }

            }
            StopMovementAnimation();
            myUnit.statistics.movementPoints = 0;
        }

        void PlayMovementAnimation()
        {
            myUnit.animator.SetBool("Walking", true);
        }
        void StopMovementAnimation()
        {
            myUnit.animator.SetBool("Walking", false);
        }

        void VisuallyMoveTowards(Tile goal)
        {
            //if we want to slowly turn, we need to ask if we already turned, and if not we turn and if yes we move here.
            TurnTowards(goal.transform.position);
            myUnit.transform.position += myUnit.transform.forward * visualSpeed * Time.deltaTime;
        }
    }
}