using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattlescapeLogic
{
    public class ActiveAreaOfEffectAttack : ActiveTileTargetAreaOfEffectAbility
    {

        [SerializeField] int damage;

        protected override void Activate()
        {
            Tile targetTile = target as Tile;
            base.Activate();
            owner.transform.LookAt(new Vector3(targetTile.transform.position.x, owner.visuals.transform.position.y, targetTile.transform.position.z));
            owner.statistics.numberOfAttacks--;
            foreach (Tile tile in GetTargetsForTile(targetTile))
            {
                if (tile.myUnit != null)
                {
                    tile.myUnit.OnHit(owner, damage);

                }
            }
        }

        public override bool IsUsableNow()
        {
            return base.IsUsableNow() && owner.CanStillAttack();
        }
    }
}