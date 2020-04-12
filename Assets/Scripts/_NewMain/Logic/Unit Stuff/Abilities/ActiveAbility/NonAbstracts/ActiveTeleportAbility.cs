using System;
using System.Collections.Generic;
using UnityEngine;

namespace BattlescapeLogic
{
    public class ActiveTeleportAbility : AbstractActiveTileTargetAbility
    {
        [Header("Teleportation Condictions")]
        [Space]
        [SerializeField] bool HasToPassObtacle;
        [SerializeField] bool HasToBeInCombat;
        [SerializeField] bool CantBeInCombat;

        protected override void DoBeforeFinish()
        {
            //Move Unit to the targetted location
        }

        public override bool IsLegalTarget(IMouseTargetable target)
        {

            Tools.TypeComparizer<IMouseTargetable, Tile>(target);

            if (base.IsLegalTarget(target))
            {
                var targetTile = target as Tile;
                return FiltersTest(targetTile) && targetTile.myObstacle == null && targetTile.myUnit == null;
            }
            else
            {
                return false;
            }
        }

        bool FiltersTest(Tile targetTile)
        {
            if (HasToPassObtacle && HasClearView(targetTile.transform.position ,0.05f, "small"))
            {
                return false;
            }
            else if (HasToBeInCombat && isInCombat() == false)
            {
                return false;
            }
            else if (CantBeInCombat && isInCombat())
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        bool isInCombat()
        {
            foreach (Tile tile in owner.GetMyTile().neighbours)
            {
                if(tile.myUnit != null && tile.myUnit.GetMyOwner() != owner.GetMyOwner())
                {
                    return true;
                }
            }

            return false;
        }
        
    }
}
