using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattlescapeLogic
{
    public class SplashDamageBuff : AbstractAttackModifierBuff, IDamageSource
    {
        [SerializeField] int splashDamage;

        //I assume this only works on Units. If not, then the structure needs a change lol. It was kinda necessary to extract important data.
        public Unit GetMyOwner()
        {
            return source.owner; 
        }

        public override void ApplyChange()
        {
            return;
        }

        public bool CanPotentiallyDamage(IDamageable target)
        {
            return true;
        }      

        public PotentialDamage GetPotentialDamageAgainst(IDamageable target)
        {
            Debug.LogWarning("This should never happen.");
            return null;
        }

        public override void ModifyAttack(IDamageable target)
        {
            List<IDamageable> alreadyDamaged = new List<IDamageable>();
            foreach (Tile tile in target.currentPosition.closeNeighbours)
            {
                if (tile.GetMyDamagableObject() != null && alreadyDamaged.Contains(tile.GetMyDamagableObject()) == false)
                {
                    Damage damage = DamageCalculator.CalculateAbilityDamage(splashDamage, this, target);                    
                    tile.GetMyDamagableObject().TakeDamage(damage);
                    alreadyDamaged.Add(tile.GetMyDamagableObject());
                    PopupTextController.AddPopupText("-" + splashDamage, PopupTypes.Damage);
                }
            }
        }

        public void OnKillUnit(Unit unit)
        {
            GetMyOwner().GetMyOwner().AddPoints(unit.statistics.cost);
        }

        protected override bool IsAcceptableTargetType(IDamageable target)
        {
            return (target is Unit);
        }

        protected override void RemoveChange()
        {
            return;
        }
    }
}