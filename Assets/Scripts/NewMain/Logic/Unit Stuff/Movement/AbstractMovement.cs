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
        protected UnitScript myUnit;
        public bool isMoving
        {
            get
            {                
                return myUnit != null && myUnit.myTile != null && Vector3.Distance(myUnit.transform.position, myUnit.myTile.transform.position) > 0.0001f;
            }
        }

        public AbstractMovement()
        {
        }

        public abstract IEnumerator MoveTo(Tile destination);

        protected void TurnTowards(Vector3 target)
        {
            Vector3 vector3 = new Vector3(target.x, myUnit.visuals.transform.position.y, target.z);
            myUnit.transform.LookAt(vector3);
            //the following line does it in, i believe, one frame. If not, then forget what i said in comments in this file :D 
            myUnit.visuals.transform.LookAt(vector3);
        }

        public void ApplyUnit(UnitScript unit)
        {
            myUnit = unit;
        }
        
    }
}