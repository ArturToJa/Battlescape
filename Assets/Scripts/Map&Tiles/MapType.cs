using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RandomMap
{
    public string Name;
    public TileType TileType;
        //Note that it only allows for ONE tile type per map for now - if we need sth different, we can maybe hardcode it after all tiles are set, or just refactor this bullshit;/
    public List<GameObject> LegalObjects;
    //this is just for checking if certain Obstacle or Doodad is 'allowed' on this map, after game wants to spawn it it checks it here.
}

