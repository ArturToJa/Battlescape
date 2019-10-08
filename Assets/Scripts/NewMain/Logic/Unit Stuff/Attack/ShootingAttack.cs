using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattlescapeLogic
{
    public class ShootingAttack : BaseAttack
    {
        //the prefab of missile shot by this unit
        [SerializeField] GameObject missilePrefab;

        //this is where EXACTLY we spawn the object - so that we dont spawn arrows in the middle of an archer etc.
        //I THINK it NEEDS to be a separate TRANSFORM, not a V3,  for ease reasons, because we can drag it in an inspector instead of copying and pasting values 
        //(i guess it is because of reference type vs value type? but i have no clue).
        [SerializeField] Transform spawningPoint;

        public ShootingAttack(Unit _myUnit) : base(_myUnit)
        {
        }

        public override void Attack(Unit target)
        {
            TurnTowardsTarget();
            PlayAttackAnimation();
        }

        protected override void PlayAttackAnimation()
        {
            sourceUnit.animator.SetTrigger("Shooting");
        }

        //Note, this has a Tile as a target and not a Unit - the reason being we might have AOE Abilities targetting 'empty' tiles (or e.g. Obstacles).
        GameObject SpawnMissile(Tile target)
        {
            GameObject newMissile = GameObject.Instantiate(missilePrefab);
            newMissile.transform.position = spawningPoint.position;
            newMissile.transform.LookAt(target.transform.position + new Vector3(0, 0.5f, 0));
            newMissile.transform.Rotate(Vector3.left, 45, Space.Self);
            //this whole rotating stuff can be SURELY simplified.
            //I just copied it from the old code cause im a lazy bastard.
            return newMissile;
        }

        IEnumerator FlyMissile(GameObject missile, Tile target)
        {
            //here we need some complex stuff! Someone smarter than me would be nice to have here ;D cause my missiles in the old BS... terrible!
            yield return null; 
        }

        // Ranged unit does nothing on it's attack animation
        public override void OnAttackAnimation()
        {
        }

        public override void OnRangedAttackAnimation()
        {
            GameObject missile = SpawnMissile(targetUnit.currentPosition);
            FlyMissile(missile, targetUnit.currentPosition);
            // if(missle == hit)
            //          sourceUnit.OnHit(target)
        }
    }
}
