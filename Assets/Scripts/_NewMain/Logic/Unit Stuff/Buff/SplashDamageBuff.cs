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
            foreach (Tile tile in Global.instance.currentMap.board)
            {
                if (target.GetDistanceTo(tile.position) == 1 && tile.GetMyDamagableObject() != null)
                {
                    PopupTextController.AddPopupText("-" + splashDamage, PopupTypes.Damage);
                    Unit owner = buffGroup.owner as Unit;
                    tile.GetMyDamagableObject().TakeDamage(owner, splashDamage);
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