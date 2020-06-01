using System;
using UnityEngine;

namespace BattlescapeLogic
{
    public class BehaviorChangeBuff : AbstractBuff
    {

        public override void ApplyChange()
        {
            return;
        }

        protected override void RemoveChange()
        {
            return;
        }

        protected override bool IsAcceptableTargetType(IDamageable target)
        {
            if(Tools.TypeComparizer<IDamageable, Unit>(target))
            {
                Unit targettedUnit = target as Unit;
                return true;
            }

            return false;
        }
    }
}
