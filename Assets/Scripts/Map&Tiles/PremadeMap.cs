using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BattlescapeLogic;

public class PremadeMap : Map
{   
    protected override void AddDropzones()
    {
        //one more thing we can do in inspector and not hardcode as we have a premade map :))
    }
    protected override void AddObstacles()
    {
        //as map is premade, do nothing ;)
        return;
    }
    protected override void SetBoard()
    {
        // In premade map we have a Board GameObject from the beginning so we can say 'add each of my kids to correct place in the array'
        Board = new Tile[mapWidth, mapHeight];
        foreach (Tile tile in GetComponentsInChildren<Tile>())
        {
            int x = Mathf.RoundToInt(tile.transform.position.x);
            int z = Mathf.RoundToInt(tile.transform.position.z);
            Board[x, z] = tile;
        }
    }
    protected override void GenerateMapData()
    {
        // we again do not need this as we do stuff differently now on premade maps
        return;
    }
    protected override void GenerateMapVisual()
    {
        // as always, return cause we do not care ;P
        return;
    }
}
