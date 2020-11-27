using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattlescapeLogic
{
    public class StateBuff : AbstractBuff
    {
        //This subclass is redundant, it just makes sure a state buff does not 'by mistake' do anything more. This works simply by the buff having right name (keyword of a state AS its BUFFNAME (not filename));
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
            return Tools.TypeComparizer<IDamageable, Unit>(target);
        }

    }
}