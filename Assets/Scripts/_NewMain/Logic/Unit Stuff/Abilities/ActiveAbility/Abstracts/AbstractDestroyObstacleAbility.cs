using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattlescapeLogic
{
    //This ability is generic enuff im not surpirsed if more than 1 unit gets in in the future... So....
    public abstract class AbstractDestroyObstacleAbility : AbstractActiveObstacleTargetAbility
    {
        [SerializeField] int maxLegalWidth;
        [SerializeField] int maxLegalHeight;
        public override void DoAbility()
        {
            //copied from AbstractAttack and ShootingAttack:
            owner.statistics.numberOfAttacks--;
            BattlescapeGraphics.ColouringTool.UncolourAllTiles();
            if (owner.GetMyOwner().HasAttacksOrMovesLeft() == false)
            {
                PopupTextController.AddPopupText("No more units can attack!", PopupTypes.Info);
            }
            AttackObstacle();
        }

        public override Color GetColourForTargets()
        {
            return Global.instance.colours.red;
        }       

        protected override bool IsLegalTarget(IMouseTargetable target)
        {
            return base.IsLegalTarget(target) && (target as Obstacle).currentPosition.size.height <= maxLegalHeight && (target as Obstacle).currentPosition.size.width <= maxLegalWidth;
        }

        protected abstract void AttackObstacle();
        
        
    }
}