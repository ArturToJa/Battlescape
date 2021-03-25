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

        public override IEnumerator MoveTo(MultiTile newPosition)
        {
            finalPosition = newPosition;
            Queue<MultiTile> path = Pathfinder.instance.GetPathFromTo(myUnit, newPosition);
            if (myUnit.IsExittingCombat(newPosition))
            {
                myUnit.ExitCombat();
                int health = myUnit.statistics.healthPoints;
                foreach (Tile neighbour in myUnit.currentPosition.closeNeighbours)
                {
                    if (myUnit.IsAlive() && neighbour.GetMyObject<Unit>() != null && myUnit.IsEnemyOf(neighbour.GetMyObject<Unit>()))
                    {
                        int damage = DamageCalculator.CalculateDamage(neighbour.GetMyObject<Unit>(), myUnit, true, 1.5f);
                        neighbour.GetMyObject<Unit>().Backstab(myUnit,damage);
                        health -= damage;
                    }
                }
                if (health > 0)
                {
                    //THe attacks will not kill us, cause calculated :P
                }
                else
                {
                    //rip, abort.
                    yield break;                    
                }
            }
            
            BattlescapeGraphics.ColouringTool.UncolourAllTiles();
            PlayMovementAnimation();
            int tileCount = path.Count;
            for (int i = 0; i < tileCount; ++i)
            {
                MultiTile temporaryGoal = path.Dequeue();
                myUnit.OnMove(myUnit.currentPosition, temporaryGoal);
                myUnit.TryToSetMyPositionTo(temporaryGoal);
                //I am aware, that for now we are still just turning into a direction in one frame. If we ever want it any other way, it needs a bit of work to set it otherwise so im not doing it now :D.                
                //if we want to slowly turn, we need to ask if we already turned, and if not we turn and if yes we move here.   
                TurnTowards(temporaryGoal.center);
                while (Vector3.Distance(myUnit.transform.position, temporaryGoal.center) > 0.0001f)
                {
                    myUnit.transform.position = Vector3.MoveTowards(myUnit.transform.position, temporaryGoal.center,visualSpeed * Time.deltaTime);
                    yield return null;
                }                
                temporaryGoal.SetMyObjectTo(myUnit);
             }
            StopMovementAnimation();
            PlayerInput.instance.isInputBlocked = false;
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