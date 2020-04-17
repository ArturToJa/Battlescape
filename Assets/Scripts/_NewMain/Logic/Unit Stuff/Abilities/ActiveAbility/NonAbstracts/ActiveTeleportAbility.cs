using System;
using System.Collections.Generic;
using UnityEngine;

namespace BattlescapeLogic
{
    public class ActiveTeleportAbility : AbstractActiveAbility
    {
        [Header("Teleportation Condictions")]
        [Space]
        [SerializeField] bool HasToBeInCombat;
        [SerializeField] bool CantBeInCombat;
        [SerializeField] bool HasToLandInCombat;
        [SerializeField] bool CantLandInCombat;

        protected override void DoBeforeFinish()
        {
            //Move Unit to the targetted location
        }

        public override bool IsLegalTarget(IMouseTargetable target)
        {
            if (target is Tile == false)
            {
                return false;
            }
            var targetTile = target as Tile;
            return ConditionsTest(targetTile) && targetTile.myObstacle == null && targetTile.myUnit == null && IsInRange(targetTile) && targetTile.IsWalkable();
        }

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
        
        bool ConditionsTest(Tile targetTile)
        {
            if (HasToBeInCombat && GetComponent<Unit>().IsInCombat() == false)
            {
                return false;
            }
            if (CantBeInCombat && GetComponent<Unit>().IsInCombat())
            {
                return false;
            }
            if(HasToLandInCombat && willLandInCombat(targetTile) == false)
            {
                return false;
            }
            if(CantLandInCombat && willLandInCombat(targetTile))
            {
                return false;
            }

            return true;
        }

        bool willLandInCombat(Tile targetTile)
        {
            foreach(Tile tile in targetTile.neighbours)
            {
                if(tile.myUnit != null && tile.myUnit.GetMyOwner() != GetComponent<Unit>().GetMyOwner())
                {
                    return true;
                }
            }
            return false;
        }
    }

        
    
}
