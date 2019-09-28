using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattlescapeLogic
{
    public class Unit : MonoBehaviour
    {
        public AbstractMovement movement { get; private set; }
        public readonly int index;
        public Player owner { get; private set; }
        public string unitName { get; private set; }
        //I know currentPosition was supposed to be private set, but now we have MovementType doing that... so it cannot, right?
        public Tile currentPosition { get; set; }
        //Again, was supposed to be private set but MovementType has to change movement points to 0;
        public Statistics statistics { get; set; }
        public List<Ability> abilities { get; private set; }
        public List<Buff> buffs { get; private set; }      
        public GameObject visuals { get; private set; }

        public void Start()
        {
            
        }

        public void Move(Tile newPosition)
        {
            movement.ApplyUnit(this);
            //StartCoroutine(myMovementType.Move(this));            
        }

        public void PlayMovementAnimation()
        {

        }
        public void StopMovementAnimation()
        {

        }    
    }
}

