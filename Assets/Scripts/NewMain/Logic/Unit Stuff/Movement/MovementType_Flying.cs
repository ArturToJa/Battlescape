using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BattlescapeLogic
{
    public class MovementType_Flying : MovementType_Base
    {
        //how high does this creature fly?
        float flyHeight;
        //IDK if we need 3 different speeds but maybe because of animation we might idk really i dont yet have a single flying unit
        float risingSpeed;
        float forwardSpeed;
        float landingSpeed;

        public MovementType_Flying(Unit _myUnit) : base(_myUnit)
        {
            myUnit = _myUnit;
        }

        public override IEnumerator MoveTo(Tile destination)
        {
            //this is what can be called retarded movement, but i cannot into realistic movement as much as Poland cannot into space ;<\
            // it flies up, then forwards, then down, in straight lines. I know it is bad!

            //turns in one frame currently
            TurnTowards(destination.transform.position);

            //while not up enough, rise
            while (myUnit.transform.position.y < flyHeight)
            {
                FlyUp();
                yield return null;
            }
            //once we have broke out of the first loop, we will not get back to it (we use yield return, not yield break)

            //while not above the target, fly towards it
            while (!IsAbove(destination.transform.position))
            {
                FlyForward();
                yield return null;
            }
            while (myUnit.transform.position.y > 0)
            {
                Land();
                yield return null;
            }
        }

        void FlyUp()
        {
            myUnit.transform.position += Vector3.up * risingSpeed * Time.deltaTime;
        }

        void FlyForward()
        {
            myUnit.transform.position += myUnit.transform.position += myUnit.transform.forward * forwardSpeed * Time.deltaTime;
        }

        void Land()
        {
            myUnit.transform.position += Vector3.down * landingSpeed * Time.deltaTime;
            //we cannot allow the unit to get LOWER than zero obviously
            if (myUnit.transform.position.y < 0)
            {
                myUnit.transform.position = new Vector3(myUnit.transform.position.z, 0, myUnit.transform.position.z);
            }
        }

        bool IsAbove(Vector3 position)
        {
            return myUnit.transform.position.x == position.x && myUnit.transform.position.z == position.z;
        }
    }
}