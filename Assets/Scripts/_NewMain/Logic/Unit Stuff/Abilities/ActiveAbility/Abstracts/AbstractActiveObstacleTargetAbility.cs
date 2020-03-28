using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BattlescapeLogic
{
    public abstract class AbstractActiveObstacleTargetAbility : AbstractActiveAbility
    {
        [Header("Statistics")]
        [Space]
        [SerializeField]
        int _range;
        protected int range
        {
            get
            {
                if (isRangeModified)
                {
                    return owner.statistics.GetCurrentAttackRange() + rangeModifier;
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

        public override void ColourPossibleTargets()
        {
            foreach (Tile tile in Global.instance.currentMap.board)
            {
                if (IsLegalTarget(tile.myObstacle))
                {
                    tile.highlighter.TurnOn(targetColouringColour);
                }
            }
        }

        public override bool IsLegalTarget(IMouseTargetable target)
        {
            if (target is Obstacle == false)
            {
                return false;
            }
            Obstacle targetObstacle = target as Obstacle;
            return IsInRange(targetObstacle);
        }

        protected bool IsInRange(Obstacle obstacle)
        {
            return obstacle.GetDistanceTo(owner.currentPosition.position) <= range;
        }
    }
}