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
        Vector3 exactMousePosition;


        protected override void Activate()
        {
            base.Activate();
            //Przenieś unita wraz z animacją
        }

        public override bool IsLegalTarget(IMouseTargetable target, Vector3 exactClickPosition)
        {

            if (target is Tile == false)
            {
                return false;
            }
            var targetTile = target as Tile;
            MultiTile position = targetTile.PositionRelatedToMouse(owner.currentPosition.width,owner.currentPosition.height,exactClickPosition);
            return ConditionsTest(position) && IsInRange(position) && position.IsWalkable();
        }

        public override void ColourPossibleTargets()
        {
            foreach(MultiTile position in Pathfinder.instance.GetAllLegalPositionsFor(owner))
            {
                if (Pathfinder.instance.IsLegalTileForUnit(position, owner, range))
                {
                    foreach(Tile tile in position)
                    {
                        tile.highlighter.TurnOn(targetColouringColour);
                    }
                }
            }
        }
        
        bool ConditionsTest(MultiTile position)
        {
            if (HasToBeInCombat && GetComponent<Unit>().IsInCombat() == false)
            {
                return false;
            }
            if (CantBeInCombat && GetComponent<Unit>().IsInCombat())
            {
                return false;
            }
            if(HasToLandInCombat && willLandInCombat(position) == false)
            {
                return false;
            }
            if(CantLandInCombat && willLandInCombat(position))
            {
                return false;
            }

            return true;
        }

        bool willLandInCombat(MultiTile targetTile)
        {
            foreach(MultiTile posiotion in targetTile.neighbours)
            {
                if(posiotion.IsProtectedByEnemyOf(owner))
                {
                    return true;
                }
            }
            return false;
        }
    }

        
    
}
