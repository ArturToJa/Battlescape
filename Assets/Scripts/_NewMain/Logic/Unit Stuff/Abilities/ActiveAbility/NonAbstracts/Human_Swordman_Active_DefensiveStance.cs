using System.Collections.Generic;
using UnityEngine;

namespace BattlescapeLogic
{
    public class Human_Swordman_Active_DefensiveStance : AbstractActiveNoTargetAbility
    {
        [SerializeField] List<GameObject> selfBuffs;


        protected override void Activate()
        {
            base.Activate();
            ApplyBuffsToUnit(selfBuffs, owner);
            owner.statistics.movementPoints = 0;
        }

        public override bool IsLegalTarget(IMouseTargetable target)
        {
            Debug.LogError("If you are asking this, most likely something went wrong?");
            return true;
        }


    }
}