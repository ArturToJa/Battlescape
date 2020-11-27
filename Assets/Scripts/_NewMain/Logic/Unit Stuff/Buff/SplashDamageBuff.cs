using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattlescapeLogic
{
    public class SplashDamageBuff : AbstractAttackModifierBuff
    {
        [SerializeField] int splashDamage;

        public override void ApplyChange()
        {
            return;
        }

        public override void ModifyAttack(IDamageable target, int damageToTarget)
        {
            List<IDamageable> alreadyDamaged = new List<IDamageable>();
            foreach (Tile tile in target.currentPosition.closeNeighbours)
            {
                if (tile.GetMyDamagableObject() != null && alreadyDamaged.Contains(tile.GetMyDamagableObject()) == false)
                {
                    PopupTextController.AddPopupText("-" + splashDamage, PopupTypes.Damage);
                    Unit owner = buffGroup.owner as Unit;
                    tile.GetMyDamagableObject().TakeDamage(owner, splashDamage);
                    alreadyDamaged.Add(tile.GetMyDamagableObject());
                }
            }            
        }

        protected override bool IsAcceptableTargetType(IDamageable target)
        {
            return true;
        }

        protected override void RemoveChange()
        {
            return;
        }
    }
}