﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BattlescapeLogic;

public static class ColouringTool
{
    static float speed = 2.0f;       

    public static void UncolourAllTiles()
    {
        Tile[] AllTiles = Object.FindObjectsOfType<Tile>();
        foreach (Tile tile in AllTiles)
        {
            UncolourTile(tile);
        }

    }

    public static void SetColour(MonoBehaviour target, Color colour)
    {
        target.StartCoroutine(SetColourRoutine(target.gameObject, colour));
    }

    static IEnumerator SetColourRoutine(GameObject target, Color colour)
    {
        Renderer renderer = target.GetComponent<Renderer>();
        while (renderer.material.color != colour)
        {
            float t = speed * Time.deltaTime;
            renderer.material.color = Color.Lerp(renderer.material.color, colour, t);
            yield return null;
        }
    }

    static void UncolourTile(Tile tile)
    {
        tile.StopAllCoroutines();
        tile.GetComponent<Renderer>().material.color = Color.white;
    }
}