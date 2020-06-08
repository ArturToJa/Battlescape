using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattlescapeLogic
{
    class ActiveBuffTargetAbility : AbstractActiveAbility
    {
        [Header("Buffs")]
        [Space]
        [SerializeField] protected List<GameObject> selfBuffs;
        [SerializeField] protected List<GameObject> targetBuffs;
        [Space]
        [SerializeField] GameObject onTargetCastVisualEffect;
        [SerializeField] bool isSelfBuff;

        public override void Start()
        {
            base.Start();
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
                ApplyBuffsToUnit(selfBuffs, owner);
            }
            else
            {
                Unit targettedUnit = target as Unit;
                ApplyBuffsToUnit(selfBuffs, owner);
                ApplyBuffsToUnit(targetBuffs, target as Unit);
                DoVisualEffectFor(onTargetCastVisualEffect, targettedUnit.gameObject);
            }
        }

        public override void ColourPossibleTargets()
        {
            foreach (Unit unit in Global.instance.GetAllUnits())
            {
                if (IsLegalTarget(unit))
                {
                    foreach (Tile tile in unit.currentPosition)
                    {
                        tile.highlighter.TurnOn(targetColouringColour);
                    }
                }
            }
        }

        public override bool IsLegalTarget(IMouseTargetable target, Vector3 exactClickPosition)
        {
            if (isSelfBuff)
            {
                return true;
            }
            else if (target is Unit)
            {
                var targettetUnit = target as Unit;
                return filter.FilterTeam(targettetUnit.GetMyOwner().team) && filter.FilterUnit(targettetUnit);
            }
            else
            {
                return false;
            }
        }

    }
}
