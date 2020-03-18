﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattlescapeLogic
{
    public abstract class AbstractPassiveSelfAbility : AbstractPassiveAbility
    {
       

        public override void OnNewRound()
        {
            ApplyBuffsToUnit(placeableBuffs, owner);
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