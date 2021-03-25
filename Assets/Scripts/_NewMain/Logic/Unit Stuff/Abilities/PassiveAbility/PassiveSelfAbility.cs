using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattlescapeLogic
{
    public class PassiveSelfAbility : AbstractPassiveAbility
    {
        protected override void Start()
        {
            base.Start();
            ApplyBuffsToUnit(placeableBuffs, owner);
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
        public override void OnNewOwnerTurn()
        {
            return;
        }

    }
}