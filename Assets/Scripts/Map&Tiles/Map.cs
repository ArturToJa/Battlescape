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
    void Start()
    {
        SetBasicStuff();        
        GenerateMapData();
        SetBoard();
        GenerateMapVisual();
        CommandAddObstacles();
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
        foreach (Tile ct in Board)
        {
            ct.isDropzoneOfPlayer = new bool[2];
            if (ct.transform.position.x < 3 && ct.hasObstacle == false)
            {
                ct.isDropzoneOfPlayer[0] = true;
            }
            else if (ct.transform.position.x > 12 && ct.hasObstacle == false)
            {
                ct.isDropzoneOfPlayer[1] = true;
            }            
        }
    }

    protected virtual void CommandAddObstacles()
    {
        if (GameStateManager.Instance.MatchType == MatchTypes.Online && PhotonNetwork.isMasterClient)
        {
            GameStateManager.Instance.GetComponent<PhotonView>().RPC("RPCSetSeed", PhotonTargets.All, Random.Range(0, 99999));
        }
        else if (GameStateManager.Instance.MatchType != MatchTypes.Online)
        {
            mapVisuals.RandomlyPutObstacles(Random.Range(0, 99999));

        }
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



}

