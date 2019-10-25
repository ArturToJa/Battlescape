using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BattlescapeLogic;

namespace BattlescapeGraphics
{
    public class TileGraphics : MonoBehaviour
    {
        Tile myTile;
        GameObject grid;

        private void Start()
        {
            myTile = GetComponentInParent<Tile>();
            grid = Helper.FindChildWithTag(gameObject, "Grid");
            grid.SetActive(false);
        }


        void Update()
        {
            if (TurnManager.Instance.TurnCount == 0 && myTile.isDropzoneOfPlayer[0])
            {
                GetComponent<Renderer>().material.color = Color.green;
            }
            else if (TurnManager.Instance.TurnCount == 0 && myTile.isDropzoneOfPlayer[1])
            {
                GetComponent<Renderer>().material.color = Color.red;
            }
            if (Input.GetKeyDown(KeyCode.F5))
            {
                grid.SetActive(!grid.activeSelf);
            }
        }
    }
}


