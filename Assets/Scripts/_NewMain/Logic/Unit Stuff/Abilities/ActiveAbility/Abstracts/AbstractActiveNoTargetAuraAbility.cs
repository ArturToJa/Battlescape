using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattlescapeLogic
{
    public abstract class AbstractActiveNoTargetAuraAbility : AbstractActiveNoTargetAbility
    {
        //This class represents an ability, that generates an aura - like a PassiveAuraAbility - around the owner of the ability, but actively, on cast, not all the time.
        //Great example of this type of ability is an ability of the hero Knight - Battlecry. It gives an attack buff to all allies in range, so is an auto-cast on click (no target per se), but is an active.

        [SerializeField] List<GameObject> auraBuffs;        

        protected override void Activate()
        {
            base.Activate();
            foreach (Unit unit in Global.instance.GetAllUnits())
            {
                if (IsLegalTarget(unit))
                {
                    ApplyBuffsToUnit(auraBuffs, unit);
                }
            }
        }       

        public override bool IsLegalTarget(IMouseTargetable target)
        {
            if ((target is Unit) == false)
            {
                return false;
            }

            var targetUnit = target as Unit;
            return IsInRange(targetUnit) && filter.FilterTeam(targetUnit.GetMyOwner().team) && filter.FilterPlayer(targetUnit.GetMyOwner()) && filter.FilterUnit(targetUnit); 
        }

        

    }
}
