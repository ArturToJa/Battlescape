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
            if(grid)
                grid.SetActive(false);
        }


        void Update()
        {
            if (TurnManager.Instance.TurnCount == 0)
            {
                if (myTile.DropzoneOfPlayer == 0)
                {
                    GetComponent<Renderer>().material.color = Color.green;
                }
                else if (myTile.DropzoneOfPlayer == 1)
                {
                    GetComponent<Renderer>().material.color = Color.red;
                }

            }            
        }

        public void ToggleGrid()
        {
            grid.SetActive(!grid.activeSelf);
        }
    }
}


