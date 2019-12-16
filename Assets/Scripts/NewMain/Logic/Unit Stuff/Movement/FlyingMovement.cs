using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BattlescapeLogic
{
    public class FlyingMovement : AbstractMovement
    {
        //how high does this creature fly?
        float flyHeight = 3f;
        //IDK if we need 3 different speeds but maybe because of animation we might idk really i dont yet have a single flying unit


        public FlyingMovement() : base()
        {
        }

        public override IEnumerator MoveTo(Tile destination)
        {
            PlayMovementAnimation();
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
            StopMovementAnimation();
            //HERE we need to pathfind, actually, to get distance etc. to know how many points we lost - I THINK. Or maybe fliers cannot move more that once per turn xD?
            myUnit.statistics.movementPoints = 0;
            myUnit.OnMove(myUnit.currentPosition, destination);
            destination.SetMyUnitTo(myUnit);
            GameStateManager.Instance.EndAnimation();
        }

        void FlyUp()
        {
            myUnit.transform.position += Vector3.up * visualSpeed * Time.deltaTime;
        }

        void FlyForward()
        {
            myUnit.transform.position += myUnit.transform.position += myUnit.transform.forward * visualSpeed * Time.deltaTime;
        }

        void Land()
        {
            myUnit.transform.position += Vector3.down * visualSpeed * Time.deltaTime;
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

        //the Animation play/stop functions may change - we may for example get custom landing/starting animation outside of general flight animation, so we may use that
        //(and if only some fliers get that, the rest may just use their only flying animation for both landing and starting).
        //that is the only reason i 'copied' these functions into both types of movement instead of making them a part of the abstract base.

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