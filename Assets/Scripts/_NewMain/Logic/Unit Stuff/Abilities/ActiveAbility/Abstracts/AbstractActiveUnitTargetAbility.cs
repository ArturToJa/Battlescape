﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattlescapeLogic
{
    public abstract class AbstractActiveUnitTargetAbility : AbstractActiveAbility
    {        
        [Header("Statistics")]
        [Space]
        [SerializeField] int _range;
        protected int range
        {
            get
            {
                if(isRangeModified)
                {
                    return rangeModifier;
                }
                else
                {
                    return _range;
                }
            }
            set
            {
                _range = value;
            }
        }
        [SerializeField] bool isRangeModified;
        [SerializeField] int rangeModifier;

        [Header("Buffs")]
        [Space]
        [SerializeField] protected List<GameObject> selfBuffs;
        [SerializeField] protected List<GameObject> targetBuffs;

        [SerializeField] GameObject onTargetCastVisualEffect;

        public override bool IsLegalTarget(IMouseTargetable target)
        {
            if ((target is Unit) == false)
            {
                return false;
            }
            Unit targetUnit = target as Unit;
            return IsInRange(targetUnit) && filter.FilterTeam(targetUnit.owner.team) && filter.FilterPlayer(targetUnit.owner) && filter.FilterUnit(targetUnit);

        }

        public bool IsInRange(Unit unit)
        {
            return owner.currentPosition.position.DistanceTo(unit.currentPosition.position) <= range;
        }

        public override void ColourPossibleTargets()
        {
            foreach (Unit unit in Global.instance.GetAllUnits())
            {
                if (IsLegalTarget(unit))
                {
                    unit.currentPosition.highlighter.TurnOn(targetColouringColour);
                }
            }
        }

        protected override void Activate()
        {
            base.Activate();
            Unit targetUnit = target as Unit;
            DoVisualEffectFor(onTargetCastVisualEffect, targetUnit.gameObject);
        }
    }
}