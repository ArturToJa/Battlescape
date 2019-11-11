using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BattlescapeLogic;

namespace BattlescapeGraphics
{
    public static class ColouringTool
    {
        static float speed = 2.0f;

        public static void UncolourAllTiles()
        {
            foreach (Tile tile in Map.Board)
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

        public static void ColourLegalTilesFor(Unit unit)
        {
            UncolourAllTiles();
            var temp = Pathfinder.instance.GetAllLegalTilesFor(unit);
            foreach (Tile tile in temp)
            {
                if (tile.IsProtectedByEnemyOf(unit))
                {
                    SetColour(tile, Color.red);
                }
                else
                {
                    SetColour(tile, Color.cyan);
                }
            }
        }
    }
}