using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattlescapeGraphics
{
    public class TileHighlighter : MonoBehaviour
    {
        Renderer myRenderer;
        GameObject grid;
       
        //THIS is instead of start cause it makes sure we run it before using the class and breaking it.
        public void OnSetup()
        {
            myRenderer = GetComponent<Renderer>();
            grid = BattlescapeLogic.Helper.FindChildWithTag(transform.parent.gameObject, "Grid");
            grid.SetActive(false);
        }

        public void TurnOn(Color colour)
        {
            myRenderer.enabled = true;
            myRenderer.material.color = colour;
        }

        public void TurnOff()
        {
            myRenderer.enabled = false;
        }

        public void ToggleGrid()
        {
            grid.SetActive(!grid.activeSelf);
        }
    }
}