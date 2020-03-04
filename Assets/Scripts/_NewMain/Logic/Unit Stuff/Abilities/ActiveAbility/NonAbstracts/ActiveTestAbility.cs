using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattlescapeLogic
{
    public class ActiveTestAbility : AbstractActiveAbility
    {
        [Space]
        [SerializeField] int damage;
        public override void Activate()
        {
            var targetUnit = target as Unit;
            owner.HitTarget(targetUnit, damage);
        }

        public override void ColourTargets()
        {
            return;
        }

        public override bool IsLegalTarget(IMouseTargetable target)
        {
            if (target is Unit == false)
            {
                return false;
            }
            else
            {
                var targetUnit = target as Unit;
                return targetUnit.owner == owner.owner;
            }
        }

        protected override bool IsUsableNow()
        {
            return GameRound.instance.currentPlayer == owner.owner;
        }
    }
}