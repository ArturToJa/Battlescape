using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattlescapeLogic
{
    public class Map
    {        
        public int mapWidth;
        public int mapHeight;

        static Map _instance;
        public static Map instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new Map();
                }
                return _instance;
            }
        }        
        public Tile[,] board;
    }
}
