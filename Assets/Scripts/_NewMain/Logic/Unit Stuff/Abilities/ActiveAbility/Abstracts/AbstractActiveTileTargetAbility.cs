using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattlescapeLogic
{
    public abstract class AbstractActiveTileTargetAbility : AbstractActiveAbility
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
                    return  owner.statistics.GetCurrentAttackRange() + rangeModifier;
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
                if (IsLegalTarget(tile))
                {
                    tile.highlighter.TurnOn(targetColouringColour);
                }
            }
        }

        public override bool IsLegalTarget(IMouseTargetable target)
        {
            if (target is Tile == false)
            {
                return false;
            }
            Tile targetTile = target as Tile;
            return IsInRange(targetTile);

        }

        protected bool IsInRange(Tile position)
        {
            return owner.currentPosition.DistanceTo(position) <= range;
        }        
       
    }
}