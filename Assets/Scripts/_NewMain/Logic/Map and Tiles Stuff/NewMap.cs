using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattlescapeLogic
{
    //TEMPORARILY renamed to "NewMap". After we delete "old" map at all, we will be able to rename this to "Map" but its just for now to not get confused ^ ^
    //THIS is NEVER used now -> needs to be implemented in some short future!
    public class NewMap
    {        
        private static NewMap _instance = new NewMap();

        static NewMap()
        {
        }
        private NewMap()
        {
        }
        public static NewMap instance
        {
            get
            {
                return _instance;
            }
        }

        public int mapWidth;
        public int mapHeight;

        public Tile[,] board;
    }
}
