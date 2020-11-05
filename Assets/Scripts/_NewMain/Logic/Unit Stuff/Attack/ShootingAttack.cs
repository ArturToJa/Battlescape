using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattlescapeLogic
{
    public class ShootingAttack : AbstractAttack, IMissileLaucher
    {
        //the prefab of missile shot by this unit

        //this is where EXACTLY we spawn the object - so that we dont spawn arrows in the middle of an archer etc.
        //I THINK it NEEDS to be a separate TRANSFORM, not a V3,  for ease reasons, because we can drag it in an inspector instead of copying and pasting values 
        //(i guess it is because of reference type vs value type? but i have no clue).
        [SerializeField] Transform spawningPoint;

        public ShootingAttack(Unit _myUnit) : base(_myUnit)
        {
            _myUnit.equipment.EquipMainRangedWeapon();
        }

        public override void Attack(IDamageable target)
        {
            base.Attack(target);
            TurnTowardsTarget();
            PlayAttackAnimation();
        }

        protected override void PlayAttackAnimation()
        {
            sourceUnit.animator.SetTrigger("Shooting");
        }

        //Note, this has a Tile as a target and not a Unit - the reason being we might have AOE Abilities targetting 'empty' tiles (or e.g. Obstacles).
        public void SpawnMissile()
        {
            Missile missile = GameObject.Instantiate(sourceUnit.myMissile, sourceUnit.transform.position, sourceUnit.transform.rotation);

            //this should actually be SPAWNING POINT on shooter, not SHOOTER POSITION (not middle of a shooter lol)
            missile.sourceUnit = sourceUnit;
            missile.target = targetObject.GetMyPosition();
            missile.myLauncher = this;
        }

        // Ranged unit does nothing on it's attack animation
        public override void OnAttackAnimation()
        {
            SpawnMissile();
        }

        public void OnMissileHitTarget()
        {
            Networking.instance.SendCommandToHit(sourceUnit, targetObject);
        }
    }
}
