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
    public abstract class AbstractAttack : IDamageSource
    {
        public string name;
        public Unit sourceUnit { get; protected set; }
        protected IDamageable targetObject;

        public AbstractAttack(Unit _myUnit)
        {
            sourceUnit = _myUnit;
        }

        public virtual void BasicAttack(IDamageable target)
        {
            sourceUnit.statistics.numberOfAttacks--;
            BattlescapeGraphics.ColouringTool.UncolourAllTiles();
            targetObject = target;
            if (sourceUnit.GetMyOwner().HasAttacksOrMovesLeft() == false)
            {
                PopupTextController.AddPopupText("No more units can attack!", PopupTypes.Info);
            }
        }

        public abstract void Backstab(IDamageable target, Damage damage);
       

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

        public PotentialDamage GetPotentialDamageAgainst(IDamageable target)
        {
            return DamageCalculator.GetPotentialDamage(Statistics.baseDamage, this, target);
        }

        public bool CanPotentiallyDamage(IDamageable target)
        {
            if (target is Unit && (target as Unit).IsEnemyOf(sourceUnit) == false) //no ally damage showing
            {
                return false;
            }
            return sourceUnit.statistics.GetCurrentMaxNumberOfAttacks() > 0;
        }                         

        public int GetAttackValue()
        {
            return sourceUnit.statistics.GetCurrentAttack();
        }

        public string GetOwnerName()
        {
            return sourceUnit.info.unitName;
        }

        public ModifierGroup GetMyDamageModifiers()
        {
            return sourceUnit.modifiers;
        }

        public void OnKillUnit(Unit unit)
        {
            if (sourceUnit.IsEnemyOf(unit))
            {
                sourceUnit.GetMyOwner().AddPoints(unit.statistics.cost);
            }
        }
    }
}
