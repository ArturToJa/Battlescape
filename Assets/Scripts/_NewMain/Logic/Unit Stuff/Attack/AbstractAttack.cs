using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BattlescapeLogic
{
    public enum AttackTypes
    {
        Melee = 0,
        Ranged = 1,
        Instant = 2
    }
    public abstract class AbstractAttack
    {
        public string name;
        protected Unit sourceUnit;
        protected Unit targetUnit;

        public AbstractAttack(Unit _myUnit)
        {
            sourceUnit = _myUnit;
        }

        public virtual void Attack(Unit target)
        {
            BattlescapeGraphics.ColouringTool.UncolourAllTiles();
            targetUnit = target;
            if (target.owner.HasAttacksOrMovesLeft() == false)
            {
                PopupTextController.AddPopupText("No more units can attack!", PopupTypes.Info);
            }
        }
       

        protected abstract void PlayAttackAnimation();

        protected void TurnTowardsTarget()
        {
            sourceUnit.visuals.transform.LookAt(new Vector3(targetUnit.transform.position.x, sourceUnit.visuals.transform.position.y, targetUnit.transform.position.z));
        }

        public abstract void OnAttackAnimation();

        public abstract void OnRangedAttackAnimation();

        public bool CanAttack(Unit targetUnit)
        {
            return
                GameRound.instance.currentPhase == TurnPhases.Attack
                && sourceUnit.owner.IsCurrentLocalPlayer()
                && sourceUnit.owner == GameRound.instance.currentPlayer
                && sourceUnit.CanStillAttack()
                && sourceUnit.IsInAttackRange(targetUnit.transform.position)
                && sourceUnit.IsEnemyOf(targetUnit);
        }        
    }
}
