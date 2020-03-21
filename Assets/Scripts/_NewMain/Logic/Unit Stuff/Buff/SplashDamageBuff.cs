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

        public override void ModifyAttack(Unit target, int damageToTarget)
        {
            foreach (Tile neighbour in target.currentPosition.neighbours)
            {
                if (neighbour.myUnit != null)
                {
                    PopupTextController.AddPopupText("-" + splashDamage, PopupTypes.Damage);
                    Unit owner = buffGroup.owner as Unit;
                    neighbour.myUnit.TakeDamage(owner, splashDamage);
                }
            }
        }

        protected override bool IsAcceptableTargetType(IDamageable target)
        {
            return Tools.TypeComparizer<IDamageable, Unit>(target);
        }

        protected override void RemoveChange()
        {
            return;
        }
    }
}