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

    public abstract class AbstractAbility : NewRound
    {
        public string name { get; protected set; }
        public string description { get; protected set; }
        public Unit owner { get; protected set; }
        public List<bool> filter { get; protected set; }

        public override void OnNewRound()
        {

        }
    }
}