using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattlescapeLogic
{
    public class GameTurn
    {
        //THere is ONE AND ONLY ONE GameTurn!
        public LinkedList<INewTurn> newTurnObjects = new LinkedList<INewTurn>();

        static GameTurn _instance;
        public static GameTurn instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new GameTurn();
                }
                return _instance;
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
            foreach (INewTurn turnObject in newTurnObjects)
            {
                turnObject.OnNewTurn();
            }
        }
    }
}

