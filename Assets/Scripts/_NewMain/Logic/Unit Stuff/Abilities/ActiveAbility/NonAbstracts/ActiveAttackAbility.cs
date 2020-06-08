using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace BattlescapeLogic
{
    public class ActiveAttackAbility : AbstractAttackAbility
    {

        [Header("Shooting Settings")]
        [Space]
        [SerializeField] bool needsClearVision;
        [SerializeField] float damageLoweredEveryMeter;

        protected override void Activate()
        {
            base.Activate();

            ApplyBuffsToUnit(selfBuffs, owner);
            CalculateFinalDamage();
            owner.attack = new AbilityAttack(this);
            owner.attack.Attack(targetedUnit);
        }

        void CalculateFinalDamage()
        {
            var targetUnit = target as Unit;
            bonusDamage -= Convert.ToInt32(Math.Ceiling((Vector3.Distance(owner.transform.position, targetUnit.transform.position) * damageLoweredEveryMeter)));
        }

        public override bool IsLegalTarget(IMouseTargetable target, Vector3 exactClickPosition)
        {
            if ((target is Unit) == false)
            {
                return false;
            }
            targetedUnit = target as Unit;
            if(needsClearVision && !HasClearView(targetedUnit.transform.position, 0.5f))
            {
                return false;
            }
            return IsInRange(targetedUnit) && filter.FilterTeam(targetedUnit.GetMyOwner().team) && filter.FilterPlayer(targetedUnit.GetMyOwner()) && filter.FilterUnit(targetedUnit);
        }

        public override void ColourPossibleTargets()
        {
            foreach (Unit unit in Global.instance.GetAllUnits())
            {
                if (IsLegalTarget(unit,Vector3.zero))
                {
                    foreach(Tile tile in unit.currentPosition)
                    {
                        tile.highlighter.TurnOn(targetColouringColour);
                    }
                }
            }
        }

        public override bool IsUsableNow()
        {
            return base.IsUsableNow() && owner.CanStillAttack();
        }

        bool IsReady()
        {
            return PlayerInput.instance.isInputBlocked = false;
        }
        

    }
}