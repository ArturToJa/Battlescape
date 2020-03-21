using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattlescapeGraphics
{
    public class TileHighlighter : MonoBehaviour
    {
        Renderer myRenderer;

        void Start()
        {
            myRenderer = GetComponent<Renderer>();
            TurnOff();
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
    }
}