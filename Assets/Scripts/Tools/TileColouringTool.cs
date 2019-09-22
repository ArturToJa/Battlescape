using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileColouringTool : MonoBehaviour
{
    float speed;
    Tile thisTile;

    void Start()
    {
        speed = 2.0f;
        thisTile = GetComponent<Tile>();
    }

    public void ColourTileFor(UnitScript unit)
    {

        if (IsTileSafeFor(thisTile, unit))
        {
            StartCoroutine(SetColour(Color.cyan));
        }
        else if (IsTileRedFor(thisTile, unit))
        {
            StartCoroutine(SetColour(Color.red));
        }
    }

    public void ColourTile(Color c)
    {
        StartCoroutine(SetColour(c));
    }

    public static void UncolourAllTiles()
    {
        Tile[] AllTiles = Object.FindObjectsOfType<Tile>();
        foreach (Tile tile in AllTiles)
        {
            tile.GetComponent<TileColouringTool>().UncolourTile();
        }

    }

    public bool IsTileSafeFor(Tile tile, UnitScript forThisUnit)
    {
        return tile.isOccupiedByPlayer[forThisUnit.OpponentID] == false && tile.isWalkable == true && tile.hasObstacle == false;
    }

    public bool IsTileRedFor(Tile tile, UnitScript forThisUnit)
    {
        return tile.isOccupiedByPlayer[forThisUnit.OpponentID] == true  && tile.isWalkable == true && tile.hasObstacle == false;
    }

    IEnumerator SetColour(Color colour)
    {
        if (GetComponent<Tile>() != null)
        {
            GetComponent<Tile>().theColor = colour;
        }
        while (GetComponent<Renderer>().material.color != colour)
        {
            float t = speed * Time.deltaTime;
            GetComponent<Renderer>().material.color = Color.Lerp(GetComponent<Renderer>().material.color, colour, t);
            yield return null;
        }
    }

    void UncolourTile()
    {
        StopAllCoroutines();
        GetComponent<Tile>().theColor = Color.white;
        GetComponent<Renderer>().material.color = Color.white;
    }
}
