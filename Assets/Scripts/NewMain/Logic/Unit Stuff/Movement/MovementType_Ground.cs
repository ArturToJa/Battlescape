using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattlescapeLogic
{
    public class MovementType_Ground : MovementType_Base
    {
        float visualSpeed;
        //ctor
        public MovementType_Ground(Unit _myUnit) : base(_myUnit)
        {
            myUnit = _myUnit;
        }

        public override IEnumerator MoveTo(Tile newPosition)
        {
            
            Queue<Tile> path = Pathfinder.instance.GetPathFromTo(myUnit, newPosition);
            for (int i = 0; i < path.Count; ++i)
            {
                Tile temporaryGoal = path.Dequeue();
                //I am aware, that for now we are still just turning into a direction in one frame. If we ever want it any other way, it needs a bit of work to set it otherwise so im not doing it now :D.                
                myUnit.currentPosition = temporaryGoal;
                while(isMoving)
                {
                    VisuallyMoveTowards(temporaryGoal);
                    yield return null;
                }
                
            }
            myUnit.OnMovementFinished();
        }

        void VisuallyMoveTowards(Tile goal)
        {
            //if we want to slowly turn, we need to ask if we already turned, and if not we turn and if yes we move here.
            TurnTowards(goal.transform.position);
            myUnit.transform.position += myUnit.transform.forward * visualSpeed * Time.deltaTime;
        }
    }
}