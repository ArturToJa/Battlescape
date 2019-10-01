using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattlescapeLogic
{
    [System.Serializable]
    public class Ability
    {
        [SerializeField] string _name;
        public string name
        {
            get
            {
                return _name;
            }
            private set
            {
                _name = value;
            }
        }
        [SerializeField] string _description;
        public string description
        {
            get
            {
                return _description;
            }
            private set
            {
                _description = value;
            }
        }
        [SerializeField] int _energyCost;
        public int energyCost
        {
            get
            {
                return _energyCost;
            }
            private set
            {
                _energyCost = value;
            }
        }
    }
}