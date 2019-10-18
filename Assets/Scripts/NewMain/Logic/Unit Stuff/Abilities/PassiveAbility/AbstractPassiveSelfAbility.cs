using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattlescapeLogic
{
    public class AbstractPassiveSelfAbility : AbstractPassiveAbility
    {
        public override void OnNewTurn()
        {
            ApplyBuffsToUnit(owner);
        }
    }
}