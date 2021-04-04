using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattlescapeLogic
{
    [System.Serializable]
    public class DefensiveStanceBuff : StatisticChangeBuff
    {
        [SerializeField] int defensePerMovePointLeft;

        int movePointsLeft;
        int attacksLeft;
        public override void ApplyChange()
        {
            Unit owner = buffGroup.owner as Unit;
            movePointsLeft = owner.statistics.movementPoints;
            attacksLeft = owner.statistics.numberOfAttacks;
            owner.statistics.bonusDefence += movePointsLeft * defensePerMovePointLeft;
            owner.statistics.ApplyBonusStatistics(statistics);
            owner.statistics.movementPoints = 0;
            owner.statistics.numberOfAttacks = 0;
            owner.statistics.numberOfRetaliations += attacksLeft;
            owner.animator.SetBool("IsDefending", true);
        }

        protected override void RemoveChange()
        {
            Unit owner = buffGroup.owner as Unit;
            owner.statistics.bonusDefence -= movePointsLeft * defensePerMovePointLeft;
            owner.statistics.RemoveBonusStatistics(statistics); 
            owner.animator.SetBool("IsDefending", false);
        }
    }
}