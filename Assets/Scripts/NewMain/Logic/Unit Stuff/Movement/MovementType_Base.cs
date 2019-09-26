using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattlescapeLogic
{
    public abstract class MovementType_Base
    {
        protected Unit myUnit;
        public bool isMoving
        {
            get
            {
                return Vector3.Distance(myUnit.transform.position, myUnit.currentPosition.transform.position) < 0.001f;
            }
        }

        public MovementType_Base(Unit _myUnit)
        {
            myUnit = _myUnit;
        }

        public abstract IEnumerator MoveTo(Tile destination);

        protected void TurnTowards(Vector3 target)
        {
            //the following line does it in, i believe, one frame. If not, then forget what i said in comments in this file :D 
            myUnit.visuals.transform.LookAt(new Vector3(target.x, myUnit.visuals.transform.position.y, target.z));
        }


    }
}