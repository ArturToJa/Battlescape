using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattlescapeLogic
{
    public class GameTurn
    {
        public LinkedList<INewTurn> newTurnObjects;

        public static GameTurn instance
        {
            get
            {
                return instance;
            }
        }
        private GameTurn()
        {

        }
        public void OnClick()
        {
            NewTurn();
        }

        private void NewTurn()
        {
            foreach(INewTurn turnObject in newTurnObjects)
            {
                turnObject.OnNewTurn();
            }
        }
    }
}

