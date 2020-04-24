using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattlescapeLogic
{
    class ActiveBuffTargetAbility : AbstractActiveAbility
    {
        [SerializeField] List<GameObject> buffs;
        [SerializeField] bool isSelfBuff;

        protected override void Start()
        {
            if (isSelfBuff)
            {
                instantActive = true;
            }
        }

        protected override void Activate()
        {
            base.Activate();
            if (isSelfBuff)
            {
                ApplyBuffsToUnit(buffs, owner);
            }
            else
            {
                ApplyBuffsToUnit(buffs, target as Unit);
            }
        }

        public override void ColourPossibleTargets()
        {
            foreach (Tile tile in Global.instance.currentMap.board)
            {
                if (tile.myUnit != null && IsLegalTarget(tile.myUnit))
                {
                    tile.highlighter.TurnOn(targetColouringColour);
                }
            }
        }

        public override bool IsLegalTarget(IMouseTargetable target)
        {
            if (isSelfBuff)
            {
                return true;
            }
            else if (target is Unit)
            {
                var targettetUnit = target as Unit;
                return filter.FilterTeam(targettetUnit.GetMyOwner().team);
            }
            else
            {
                return false;
            }
        }

    }
}
