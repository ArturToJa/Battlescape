using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattlescapeGraphics
{
    [System.Serializable]
    public class Colours
    {

        [SerializeField] Color _red;
        public Color red
        {
            get
            {
                return _red;
            }
        }

        [SerializeField] Color _green;
        public Color green
        {
            get
            {
                return _green;
            }
        }

        [SerializeField] Color _yellow;
        public Color yellow
        {
            get
            {
                return _yellow;
            }
        }

        [SerializeField] Color _blue;
        public Color blue
        {
            get
            {
                return _blue;
            }
        }
    }
}