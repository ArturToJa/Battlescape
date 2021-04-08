using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattlescapeLogic
{
    public enum MovementTypes
    {
        Ground = 0,
        Flying = 1,
        None = 2,
        Teleport = 3
    }
    public abstract class AbstractMovement
    {
        protected Unit myUnit;
        protected float visualSpeed;
        protected MultiTile finalPosition;


        //This is ONLY about finishing the WHOLE movement!
        public bool isMoving
        {
            get
            {
                return myUnit != null && myUnit.currentPosition != null && finalPosition != null && myUnit.currentPosition.Equals(finalPosition);
            }

        }

        public AbstractMovement()
        {
        }

        public abstract IEnumerator MoveTo(MultiTile destination);

        public void TurnTowards(Vector3 target)
        {
            Vector3 vector3 = new Vector3(target.x, myUnit.visuals.transform.position.y, target.z);
            //the following line does it in, i believe, one frame. If not, then forget what i said in comments in this file :D 
            myUnit.visuals.transform.LookAt(vector3);
        }

        public void ApplyUnit(Unit unit)
        {
            myUnit = unit;
        }

        public bool CanMoveTo(MultiTile destination)
        {            
            return
                destination != null
                && myUnit.GetMyOwner().IsCurrentLocalPlayer()
                && myUnit.GetMyOwner() == GameRound.instance.currentPlayer
                && GameRound.instance.currentPhase == TurnPhases.Movement
                && myUnit.CanStillMove()
                && Pathfinder.instance.IsLegalTileForUnit(destination, myUnit)
                && PlayerInput.instance.isLocked == false;
        }

    }
}