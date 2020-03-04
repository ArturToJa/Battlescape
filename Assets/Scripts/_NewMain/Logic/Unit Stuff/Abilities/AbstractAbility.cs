using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattlescapeLogic
{
    enum AbilityFilter
    {
        // team based filter
        Ally,
        Enemy,

        // player based filter
        SelfPlayer,
        OtherPlayer,

        // can affect self or not
        Self,

        // unit specific filter
        Hero,
        Regular,

        // attack type filter
        Ranged,
        Melee,

        // movement type filter
        Ground,
        Flying
    };

    public abstract class AbstractAbility : TurnChangeMonoBehaviour
    {

        [SerializeField] string _abilityName;
        public string abilityName
        {
            get
            {
                return _abilityName;
            }
            protected set
            {
                _abilityName = value;
            }
        }

        [SerializeField] string _description;
        public string description
        {
            get
            {
                return _description;
            }
            protected set
            {
                _description = value;
            }
        }

        [SerializeField] Sprite _icon;
        public Sprite icon
        {
            get
            {
                return _icon;
            }
            protected set
            {
                _icon = value;
            }
        }

        public Unit owner { get; set; }

        [SerializeField] List<bool> _filter;
        public List<bool> filter
        {
            get
            {
                return _filter;
            }
            protected set
            {
                _filter = value;
            }
        }        

        public override void OnNewRound()
        {
            return;
        }
        public override void OnNewTurn()
        {
            return;
        }
        public override void OnNewPhase()
        {
            return;
        }
    }
}