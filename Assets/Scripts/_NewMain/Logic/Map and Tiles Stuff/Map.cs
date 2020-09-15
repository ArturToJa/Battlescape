using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattlescapeLogic
{
    [System.Serializable]
    public class Map
    {
        [SerializeField] string _mapName;
        public string mapName
        {
            get
            {
                return _mapName;
            }
            private set
            {
                _mapName = value;
            }
        }
        [SerializeField] int _mapWidth;
        public int mapWidth
        {
            get
            {
                return _mapWidth;
            }
            private set
            {
                _mapWidth = value;
            }
        }

        [SerializeField] int _mapHeight;
        public int mapHeight
        {
            get
            {
                return _mapHeight;
            }
            private set
            {
                _mapHeight = value;
            }
        }

        public Tile[,] board { get; private set; }

        [SerializeField] MapVisualsGenerator _mapVisuals;
        public MapVisualsGenerator mapVisuals
        {
            get
            {
                return _mapVisuals;
            }
            private set
            {
                _mapVisuals = value;
            }
        }

        [SerializeField] bool manualObstacles;

        public void OnSetup()
        {
            GenerateBoard();
            GenerateMapVisuals();
        }

        private void GenerateMapVisuals()
        {
            if (manualObstacles == false)
            {
                Networking.instance.SendCommandToAddObstacles();
            }
        }

        private void GenerateBoard()
        {
            board = new Tile[mapWidth, mapHeight];

            foreach (Tile tile in Object.FindObjectsOfType<Tile>())
            {
                tile.OnSetup();
                board[tile.position.x, tile.position.z] = tile;
            }
        }

        public void ToggleGrid()
        {
            foreach (Tile tile in board)
            {
                BattlescapeGraphics.TileGraphics tileGraphics = tile.GetComponent<BattlescapeGraphics.TileGraphics>();
                tileGraphics.ToggleGrid();
            }
        }
    }
}
