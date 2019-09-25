using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattlescapeLogic
{
    public class Unit : MonoBehaviour
    {
        public readonly int index;
	    public Player owner { get; private set; }
        public string unitName { get; private set; }
        public Tile currentPosition { get; private set; }
        public Statistics statistics { get; private set; }
        public List<Ability> abilities { get; private set; }
        public List<Buff> buffs { get; private set; }

        public void Start()
        {

        }
    }
}

