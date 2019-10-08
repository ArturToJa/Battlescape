using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattlescapeLogic
{
    public class Global : MonoBehaviour
    {
        public static Global instance;
        public Map map { get; private set; }
        public List<PlayerTeam> playerTeams { get; private set; }

        public List<AbstractMovement> movementTypes;

        void Start()
        {
            if(instance == null)
            {
                instance = this;
            }
            else
            {
                Debug.LogError("More than one instance of Global is forbidden");
                Destroy(this);
            }
            movementTypes[(int)MovementTypes.Ground] = new GroundMovement();
            movementTypes[(int)MovementTypes.Flying] = new FlyingMovement();
            movementTypes[(int)MovementTypes.None] = null;
        }

    }
}
