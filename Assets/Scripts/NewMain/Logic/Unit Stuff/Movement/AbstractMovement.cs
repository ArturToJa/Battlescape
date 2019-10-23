using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattlescapeLogic
{
    public enum MovementTypes
    {
        Ground = 0,
        Flying = 1,
        None = 2
    }
    public abstract class AbstractMovement
    {
        protected Unit myUnit;
        public bool isMoving
        {
            get
            {
                return Vector3.Distance(myUnit.transform.position, myUnit.currentPosition.transform.position) < 0.001f;
            }
        }

        public AbstractMovement()
        {
        }

        public abstract IEnumerator MoveTo(Tile destination);

        protected void TurnTowards(Vector3 target)
        {
            //the following line does it in, i believe, one frame. If not, then forget what i said in comments in this file :D 
            myUnit.visuals.transform.LookAt(new Vector3(target.x, myUnit.visuals.transform.position.y, target.z));
        }

        public void ApplyUnit(Unit unit)
        {
            myUnit = unit;
        }
        
    }
}