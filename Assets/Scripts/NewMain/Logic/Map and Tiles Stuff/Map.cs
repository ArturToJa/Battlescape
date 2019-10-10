using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattlescapeLogic
{
    public class Map
    {        
        private static Map _instance = new Map();

        static Map()
        {
        }
        private Map()
        {
        }
        public static Map instance
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
