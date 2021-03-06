﻿using System;
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
        protected IDamageable targetObject;

        public AbstractAttack(Unit _myUnit)
        {
            sourceUnit = _myUnit;
        }

        public virtual void Attack(IDamageable target)
        {
            BattlescapeGraphics.ColouringTool.UncolourAllTiles();
            targetObject = target;
            if (sourceUnit.GetMyOwner().HasAttacksOrMovesLeft() == false)
            {
                PopupTextController.AddPopupText("No more units can attack!", PopupTypes.Info);
            }
        }
       

        protected abstract void PlayAttackAnimation();

        protected void TurnTowardsTarget()
        {
            sourceUnit.visuals.transform.LookAt(new Vector3(targetObject.GetMyPosition().x, sourceUnit.visuals.transform.position.y, targetObject.GetMyPosition().z));
            sourceUnit.visuals.transform.Rotate(new Vector3(0, sourceUnit.attackRotation, 0));
        }

        public abstract void OnAttackAnimation();

        public bool CanAttack(IDamageable targetObject)
        {
            return
                GameRound.instance.currentPhase == TurnPhases.Attack
                && sourceUnit.GetMyOwner().IsCurrentLocalPlayer()
                && sourceUnit.GetMyOwner() == GameRound.instance.currentPlayer
                && sourceUnit.CanStillAttack()
                && sourceUnit.IsInAttackRange(targetObject.currentPosition.DistanceTo(sourceUnit.currentPosition))
                && sourceUnit.IsEnemyOf(targetObject);
        }        
    }
}
