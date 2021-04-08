using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattlescapeLogic
{
    public class HideInShadows : AbstractActiveMultiTileTargetAbility
    {
        public override void DoAbility()
        {
            AbstractMovement ownerNormalMovement = owner.movement;
            owner.movement = Global.instance.movementTypes[(int)MovementTypes.Teleport];
            owner.movement.ApplyUnit(owner);
            StartCoroutine(owner.movement.MoveTo(target));
            owner.movement = ownerNormalMovement;           
        }

        protected override string GetLogMessage()
        {
            return base.GetLogMessage() + " It teleports him to a different safe location!";
        }
    }
}