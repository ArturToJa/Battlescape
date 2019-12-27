using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using BattlescapeLogic;

public class Map : MonoBehaviour
{
    
    int[,] tiles;
    public static int mapHeight = 11;
    public static int mapWidth = 16;
    MapVisuals mapVisuals;
    public static Tile[,] Board;
    //TOTALLY TEMPORARY THING hardcoded cause im lazy
    public static Vector3 MapMiddle = new Vector3(7.5f, 0, 4.5f);    
    void Start()
    {
        SetBasicStuff();        
        GenerateMapData();
        SetBoard();
        GenerateMapVisual();
        AddObstacles();
        AddDropzones();
    }


    protected void SetBasicStuff()
    {
        mapVisuals = GetComponent<MapVisuals>();
        if (PersistantMapData.hasChangedAnything)
        {
            mapVisuals.SetData();
            mapWidth = PersistantMapData.mapWidth;
            mapHeight = PersistantMapData.mapHeight;

        }
    }

    protected virtual void AddDropzones()
    {
        foreach (Tile tile in Board)
        {
            if (tile.transform.position.x < 3 && tile.hasObstacle == false)
            {
                tile.DropzoneOfPlayer = 0;
            }
            else if (tile.transform.position.x > 12 && tile.hasObstacle == false)
            {
                tile.DropzoneOfPlayer = 1;
            }
            else
            {
                tile.DropzoneOfPlayer = -1;
            }
        }
    }

    protected virtual void AddObstacles()
    {
        Networking.instance.SendCommandToAddObstacles();
    }    
    protected virtual void SetBoard()
    {
        Board = new Tile[mapWidth, mapHeight];
    }

    protected virtual void GenerateMapData()
    {

        // really old code, no time to improve it, just cut out old bad parts and didnt improve it - it makes no sense now but works still.

        tiles = new int[mapWidth, mapHeight];

        // Create tiles.
        for (int x = 0; x < mapWidth; x++)
        {
            for (int z = 0; z < mapHeight; z++)
            {
                tiles[x, z] = 0;
            }
        }
    }

   protected virtual void GenerateMapVisual()
    {
        for (int x = 0; x < mapWidth; x++)
        {
            for (int z = 0; z < mapHeight; z++)
            {
                TileType tt = mapVisuals.tileTypes[tiles[x, z]];
                GameObject go = (GameObject)Instantiate(tt.tileVisualPrefab, new Vector3(x, 0, z), Quaternion.identity);
                Tile ct = go.GetComponent<Tile>();
                Board[Mathf.RoundToInt(ct.transform.position.x), Mathf.RoundToInt(ct.transform.position.z)] = ct;
            }
        }

    }

    public Vector3 TileCoordToWorldCoord(int x, int z)
    {
        return new Vector3(x, 0, z);
    }

    public static void ToggleGrid()
    {
        foreach (Tile tile in Board)
        {
            BattlescapeGraphics.TileGraphics tileGraphics = tile.GetComponent<BattlescapeGraphics.TileGraphics>();
            tileGraphics.ToggleGrid();
        }
    }



}

