using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattlescapeLogic
{
    public abstract class AbstractActiveNoTargetAbility : AbstractActiveAbility
    {
        public override void OnClickIcon()
        {
            //I think there is no need to set currentAbility to this as it will momentarily be un-done as a No-Target-Ability self-activates when clicked.
            //Also that would possibly make us ask about legal target- while clearly there is no target.
            Activate();
        }

        public override void ColourPossibleTargets()
        {
            Debug.LogError("This kinda shouldnt happen? Or maybe im wrong");
        }
    }
}